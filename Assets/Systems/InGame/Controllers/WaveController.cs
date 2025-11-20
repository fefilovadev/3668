using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaveController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private WaveScenarioConfig scenarioConfig;
    [SerializeField] private WaveMultConfig waveMultConfig;
    [SerializeField] private EnemyConfig baseEnemyConfig;
    [SerializeField] private EnemyConfig baseBossConfig;
    [SerializeField] private WaveUI waveUI;

    private WaveConfigurator waveConfigurator;
    private int currentWave = 0;
    private bool isRunning;

    private bool _isWaveActive;
    public bool IsWaveActive => _isWaveActive;

    public event Action<bool> OnWaveStateChanged;
    public event Action<int> OnWaveStarted;
    public event Action<int, int> OnWaveStartedForDrop;
    public event Action<int> OnWaveCompleted;
    public UnityEvent OnScenarioCompleted;

    private bool bossSpawned = false;

    private void Awake()
    {
        waveConfigurator = new WaveConfigurator(waveMultConfig);

        if (waveUI != null)
        {
            OnWaveStarted += (waveNum) => waveUI.OnWaveStarted(waveNum, scenarioConfig.WavesCount);
            OnWaveCompleted += (waveNum) => waveUI.OnWaveEnded();
        }
    }

    public void StartGame()
    {
        if (isRunning) return;

        isRunning = true;
        currentWave = 0;

        if (waveUI != null)
            StartCoroutine(StartCountdownAndRunScenario());
        else
            StartCoroutine(RunScenario());
    }

    private IEnumerator StartCountdownAndRunScenario()
    {
        yield return StartCoroutine(waveUI.StartCountdownCoroutine());
        yield return RunScenario();
    }

    private IEnumerator RunScenario()
    {
        while (currentWave < scenarioConfig.WavesCount)
        {
            int nextWave = currentWave + 1;
            bool isBossWave = scenarioConfig.BossWaves.Contains(nextWave);

            int enemiesCount = isBossWave
                ? 1
                : scenarioConfig.EnemiesPerFirstWave + Mathf.RoundToInt(scenarioConfig.EnemiesCountScaling * (nextWave - 1));

            OnWaveStarted?.Invoke(nextWave);
            OnWaveStartedForDrop?.Invoke(nextWave, enemiesCount);

            _isWaveActive = true;
            OnWaveStateChanged?.Invoke(_isWaveActive);

            if (waveUI != null)
                waveUI.OnWaveStarted(nextWave, scenarioConfig.WavesCount, isBossWave);

            EnemyConfig enemyConfig = waveConfigurator.GetEnemyConfigForWave(baseEnemyConfig, nextWave);
            EnemyConfig bossConfig = waveConfigurator.GetBossConfigForWave(baseBossConfig, nextWave);
            enemySpawner.SetConfigs(enemyConfig, bossConfig);

            if (isBossWave)
            {
                enemySpawner.SpawnBoss();
                yield return StartCoroutine(WaitForEnemiesToBeCleared());
            }
            else
            {
                yield return StartCoroutine(SpawnWaveEnemies(enemiesCount));
                yield return StartCoroutine(WaitForEnemiesToBeCleared());
            }

            OnWaveCompleted?.Invoke(nextWave);
            AchievementObserver.Instance.Trigger(3, 1); //first wave (3)
            _isWaveActive = false;
            OnWaveStateChanged?.Invoke(_isWaveActive);

            if (waveUI != null)
                waveUI.OnWaveEnded(isBossWave);

            currentWave++;

            if (currentWave < scenarioConfig.WavesCount)
                yield return new WaitForSeconds(scenarioConfig.TimeBetweenWaves);
        }

        OnScenarioCompleted?.Invoke();
        isRunning = false;
    }


    private IEnumerator SpawnWaveEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            enemySpawner.SpawnEnemy();
            if (waveUI != null)
                waveUI.OnEnemySpawnedInWave(i + 1, count);

            yield return new WaitForSeconds(scenarioConfig.SpawnInterval);
        }
    }

    private IEnumerator WaitForEnemiesToBeCleared()
    {
        while (true)
        {
            EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
            bool anyActive = false;

            foreach (var e in enemies)
            {
                if (e.gameObject.activeInHierarchy)
                {
                    anyActive = true;
                    break;
                }
            }

            if (!anyActive)
                yield break;

            yield return new WaitForSeconds(2f);
        }
    }
}

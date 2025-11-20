using UnityEngine;
public class WaveConfigurator
{
    private WaveMultConfig waveMultConfig;
    public WaveConfigurator(WaveMultConfig _waveMultConfig)
    {
        waveMultConfig = _waveMultConfig; 
    }
    public EnemyConfig GetEnemyConfigForWave(EnemyConfig _enemyConfig, int _waveNumber)
    {
        EnemyConfig enemyWaveConfig = ScriptableObject.Instantiate(_enemyConfig);

        enemyWaveConfig.HealthMultiplier += waveMultConfig.EnemyHealthMultiplierPerWave * (_waveNumber - 1);
        enemyWaveConfig.DamageMultiplier += waveMultConfig.EnemyDamageMultiplierPerWave * (_waveNumber - 1);
        enemyWaveConfig.SpeedMultiplier += waveMultConfig.EnemySpeedMultiplierPerWave * (_waveNumber - 1);

        return enemyWaveConfig;
    }

    public EnemyConfig GetBossConfigForWave(EnemyConfig _bossConfig, int _waveNumber)
    {
        EnemyConfig bossWaveConfig = ScriptableObject.Instantiate(_bossConfig);

        bossWaveConfig.HealthMultiplier += waveMultConfig.BossHealthMultiplierPerWave * (_waveNumber - 1);
        bossWaveConfig.DamageMultiplier += waveMultConfig.BossDamageMultiplierPerWave * (_waveNumber - 1);
        bossWaveConfig.SpeedMultiplier += waveMultConfig.BossSpeedMultiplierPerWave * (_waveNumber - 1);

        return bossWaveConfig;
    }
}

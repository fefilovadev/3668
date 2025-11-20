using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private GameObject losePopUp;
    [SerializeField] private GameObject winPopUp;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartlButton;
    [SerializeField] private Image starsImage;
    [SerializeField] private FarmScript farm;

    private int LevelId;

    public void Win()
    {
        gameObject.GetComponent<UIPanel>().FadeIn();
        Time.timeScale = 0;
        winPopUp.SetActive(true);

        ELevelStates state = GetStarFill();
        LevelId = SceneManager.GetActiveScene().buildIndex;

        LevelSaveService levelSaveService = new LevelSaveService(9);

        ELevelStates? prevState = levelSaveService.GetLevel(LevelId - 1);

        if ((int)state > (int)prevState)
        {
            levelSaveService.UpdateLevel(LevelId - 1, state);
        }

        nextLevelButton.gameObject.SetActive(NextLevelPossible(state));

        if (farm.beenDamaged == false)
            AchievementObserver.Instance.Trigger(15, 1);

        restartlButton.interactable = PlayerInventory.Charges >= 2;
    }


    public void Lose()
    {
        gameObject.GetComponent<UIPanel>().FadeIn();
        Time.timeScale = 0;
        nextLevelButton.gameObject.SetActive(false);
        losePopUp.SetActive(true);
        restartlButton.interactable = PlayerInventory.Charges >= 2;
    }

    private bool NextLevelPossible(ELevelStates state)
    {
        string nextLevelSceneName = "Level" + (LevelId + 1);

        return PlayerInventory.Charges >= 2
               && SceneExists(nextLevelSceneName)
               && state == ELevelStates.ThreeStars;
    }
    private ELevelStates GetStarFill()
    {
        float fraction = farm.DefaultHealth / farm.Health;

        if (fraction >= 2f / 3f)
        {
            starsImage.fillAmount = 1f;
            AchievementObserver.Instance.Trigger(8, 1); //triple star (8)
            if (LevelId == 9) AchievementObserver.Instance.Trigger(14, 1); //elite farmer (14)
            return ELevelStates.ThreeStars;
        }
        else if (fraction >= 1f / 3f)
        {
            starsImage.fillAmount = 2f / 3f;
            return ELevelStates.TwoStars;
        }
        else if (fraction > 0f)
        {
            starsImage.fillAmount = 1f / 3f;
            return ELevelStates.OneStar;
        }
        else
        {
            starsImage.fillAmount = 0;
            return ELevelStates.Opened;
        }
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
                return true;
        }
        return false;
    }
}

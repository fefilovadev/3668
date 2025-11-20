using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    private float previousTimeScale = 1f;

    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void ToggleGameSpeed()
    {
        if (Time.timeScale == 0) return;
        Time.timeScale = Time.timeScale == 1 ? 2 : 1;
    }

    public void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = previousTimeScale > 0 ? previousTimeScale : 1f;
        }
    }

    public void LoadNextLevel()
    {
        PlayerInventory.Charges -= 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartScene()
    {
        PlayerInventory.Charges -= 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }


}

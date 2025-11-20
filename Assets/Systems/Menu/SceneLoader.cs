using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public float Delay = 1f;
    public void LoadSceneWithDelay(string sceneName)
    {
        StartCoroutine(LoadRoutine(sceneName));
        if (sceneName == "Menu") PlayerPrefs.SetInt("Onboarded", 1);
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        yield return new WaitForSeconds(Delay);
        SceneManager.LoadScene(sceneName);
    }
}

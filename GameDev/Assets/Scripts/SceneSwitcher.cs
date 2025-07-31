using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        Debug.Log("Scene Switches to \"Game\"");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        Debug.Log("Scene Switches to \"Main Menu\"");
    }

    public void LoadEnding()
    {
        SceneManager.LoadSceneAsync(2);
        Debug.Log("Scene Switches to \"Ending\"");
        StartCoroutine(Wait(5));
        SceneManager.LoadSceneAsync(0);
        Debug.Log("Scene Switches back to \"Main Menu\"");
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}

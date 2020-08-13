using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int index;

    private void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(index + 1);
    }

    public void LoadNextSceneAfterTime(float _waitingTime)
    {
        StartCoroutine(WaitForTimeToLoadNextScene(_waitingTime));
    }

    public void LoadSpecificSceneAfterTime(float _waitingTime, string _sceneName)
    {
        StartCoroutine(WaitForTimeToLoadSpecificScene(_waitingTime, _sceneName));
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator WaitForTimeToLoadNextScene(float _waitingTime)
    {
        yield return new WaitForSeconds(_waitingTime);
        LoadNextScene();
    }

    private IEnumerator WaitForTimeToLoadSpecificScene(float _waitingTime, string _sceneName)
    {
        yield return new WaitForSeconds(_waitingTime);
        LoadScene(_sceneName);
    }

}

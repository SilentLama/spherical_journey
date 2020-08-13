using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionScreen : MonoBehaviour
{
    [SerializeField] Slider slider = null;
    [SerializeField] SceneLoader sceneLoader = null;

    [SerializeField] float loadingTime = 5f;

    private float internalLoadingTime = 0f;

    private void Start()
    {
        //internalLoadingTime = loadingTime;
        StartCoroutine(LoadingSequence());
    }

    private void Update()
    {
        internalLoadingTime += Time.deltaTime;
        slider.value = internalLoadingTime / loadingTime;
    }


    private IEnumerator LoadingSequence()
    {
        
        yield return new WaitForSeconds(loadingTime);
        sceneLoader.LoadNextScene();
    }
}

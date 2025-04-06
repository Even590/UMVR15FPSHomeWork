using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider loadingSlider;
    public GameObject loadingCanvas;

    private void Start() {
        loadingCanvas.SetActive(false);
    }

    public void PlayGame()
    {
        StartCoroutine(LoadGameSceneAsync());
    }

    private IEnumerator LoadGameSceneAsync()
    {
        loadingCanvas.SetActive(true);
        Debug.Log($"讀取場景中");

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
        asyncOperation.allowSceneActivation = false;

        while(!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;

            if(asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingCanvas.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

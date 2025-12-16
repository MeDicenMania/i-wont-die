using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    public Slider progressBar;
    public CanvasGroup fadePanel;
    public float fadeDuration = 0.5f;

    private void Start()
    {
        StartCoroutine(InitializeAndStart());
    }

    private IEnumerator InitializeAndStart()
    {
        yield return StartCoroutine(FadeIn());

        if (progressBar != null)
            progressBar.value = 0f;

        string target = GameManager.Instance.ConsumeSceneToLoad();
        if (string.IsNullOrEmpty(target))
            target = "Main Menu";

        yield return StartCoroutine(LoadSceneAsync(target));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        float minimumLoadTime = 1.5f;
        float timer = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            bool sceneReady = operation.progress >= 0.9f;
            if (sceneReady && timer >= minimumLoadTime)
            {
                yield return StartCoroutine(FadeOut());
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = 1f - (t / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 0f;
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadePanel.alpha = t / fadeDuration;
            yield return null;
        }
        fadePanel.alpha = 1f;
    }
}

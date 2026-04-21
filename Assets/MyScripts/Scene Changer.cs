using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1.5f;
    public float blackHoldTime = 1.2f;

    void Start()
    {
        SetBlackInstant(false);
        StartCoroutine(FadeIn());
    }

    public void LoadScene1()
    {
        StartCoroutine(LoadScene("City"));
    }

    public void LoadScene2()
    {
        StartCoroutine(LoadScene("driving school"));
    }

    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(blackHoldTime);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Quit()
    {
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(blackHoldTime);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = fadePanel.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 1;
        fadePanel.color = c;
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadePanel.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 0;
        fadePanel.color = c;
    }

    void SetBlackInstant(bool black)
    {
        if (!fadePanel) return;

        Color c = fadePanel.color;
        c.a = black ? 1 : 0;
        fadePanel.color = c;
    }
}
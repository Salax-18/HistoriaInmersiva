using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SecuenciaRegresion : MonoBehaviour
{
    [Header("Fade / Parpadeo")]
    public Image fadePanel;

    [Header("Transición")]
    public string nextSceneName = "Escena2";

    [Header("Configuración")]
    public float blinkDuration = 0.25f;
    public float blinkPause = 0.15f;

    private bool regressionStarted = false;

    public void StartRegression()
    {
        if (regressionStarted)
            return;

        regressionStarted = true;

        Debug.Log("REGRESIÓN INICIADA");

        StartCoroutine(BlinkTransition());
    }

    private IEnumerator BlinkTransition()
    {
        //  transparentes
        SetAlpha(0f);

        // PARPADEO 1
        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(blinkPause);

        yield return StartCoroutine(FadeToClear());
        yield return new WaitForSeconds(blinkPause);

        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(blinkPause);

        yield return StartCoroutine(FadeToClear());
        yield return new WaitForSeconds(blinkPause);

        
        yield return StartCoroutine(FadeToBlack());

        // Esperar un poquito con la pantalla negra
        yield return new WaitForSeconds(0.5f);

        // Cambiar de escena
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, elapsed / blinkDuration);

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(1f);
    }

    private IEnumerator FadeToClear()
    {
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / blinkDuration);

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        if (fadePanel != null)
        {
            Color color = fadePanel.color;
            color.a = alpha;
            fadePanel.color = color;
        }
    }
}
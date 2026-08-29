using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerDeathEffect : MonoBehaviour
{
    [Header("CÁMARA")]
    public Camera playerCamera;

    [Range(0f, 1f)]
    public float shakeForce = 0.25f;

    [Tooltip("Cuánto dura el temblor de la cámara.")]
    public float shakeDuration = 2f;

    [Header("PANTALLA BLANCA")]
    public Image whiteFlash;

    [Tooltip("Tiempo que tarda la pantalla en ponerse blanca.")]
    public float flashFadeIn = 0.08f;

    [Header("SONIDOS")]
    public AudioClip earthquakeSound;
    public AudioClip glassBreakSound;

    [Range(0f, 1f)]
    public float soundVolume = 1f;

    [Header("TIEMPOS DEL EFECTO")]
    [Tooltip("Tiempo que pasa desde que empiezan los vidrios hasta cortar todo el audio.")]
    public float silenceDelay = 0.15f;

    private AudioSource audioSource;
    private Vector3 originalCameraPosition;
    private bool isPlaying = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (playerCamera != null)
            originalCameraPosition = playerCamera.transform.localPosition;

        // La pantalla inicia invisible.
        if (whiteFlash != null)
        {
            Color c = whiteFlash.color;
            c.a = 0f;
            whiteFlash.color = c;
        }
    }

    // Llama esta función cuando interactúes con la puerta.
    public void PlayEffect()
    {
        if (isPlaying) return;

        StartCoroutine(EffectSequence());
    }

    IEnumerator EffectSequence()
    {
        isPlaying = true;

        // -------------------------
        // TERREMOTO (SONIDO EN LOOP)
        // -------------------------
        if (earthquakeSound != null)
        {
            audioSource.clip = earthquakeSound;
            audioSource.loop = true;
            audioSource.volume = soundVolume;
            audioSource.Play();
        }

        // -------------------------
        // SHAKE DE CÁMARA
        // -------------------------
        float timer = 0f;

        while (timer < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeForce;
            float y = Random.Range(-1f, 1f) * shakeForce;

            playerCamera.transform.localPosition =
                originalCameraPosition + new Vector3(x, y, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        // Regresa la cámara a su posición original.
        playerCamera.transform.localPosition = originalCameraPosition;

        // Detener el sonido del terremoto.
        audioSource.Stop();

        // -------------------------
        // VIDRIOS ROMPIÉNDOSE
        // -------------------------
        if (glassBreakSound != null)
        {
            audioSource.PlayOneShot(glassBreakSound, soundVolume);
        }

        // Pantalla blanca inmediatamente.
        StartCoroutine(ShowWhiteScreen());

        // Espera el tiempo configurado antes de cortar todos los sonidos.
        yield return new WaitForSeconds(silenceDelay);

        // Corta TODOS los sonidos del juego.
        AudioListener.pause = true;

        isPlaying = false;
    }

    IEnumerator ShowWhiteScreen()
    {
        if (whiteFlash == null)
            yield break;

        Color c = whiteFlash.color;
        float timer = 0f;

        while (timer < flashFadeIn)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / flashFadeIn);
            whiteFlash.color = c;
            yield return null;
        }

        // Se queda completamente blanca.
        c.a = 1f;
        whiteFlash.color = c;
    }

    // Si más adelante quieres volver a activar el sonido.
    public void RestoreAudio()
    {
        AudioListener.pause = false;
    }
}
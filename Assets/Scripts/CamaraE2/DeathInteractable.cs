using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DeathInteractable : Interactable
{
    [Header("Sonidos")]
    public AudioClip houseCollapseSound;
    public AudioClip glassBreakSound;

    [Range(0f, 1f)]
    public float deathVolume = 1f;

    [Header("Pantalla Blanca")]
    public Image whiteFlash;

    [Header("Cámara")]
    public float shakeForce = 0.3f;
    public float shakeDuration = 1.5f;

    private AudioSource audioSource;
    private Vector3 originalCameraPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // playerCamera viene del script Interactable
        if (playerCamera != null)
            originalCameraPosition = playerCamera.transform.localPosition;

        if (whiteFlash != null)
        {
            Color c = whiteFlash.color;
            c.a = 0f;
            whiteFlash.color = c;
        }
    }

    // IMPORTANTE: NO usar override.
    public new void Interact()
    {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // Sonido de derrumbe
        if (houseCollapseSound != null)
        {
            audioSource.clip = houseCollapseSound;
            audioSource.loop = true;
            audioSource.volume = deathVolume;
            audioSource.Play();
        }

        // Shake sincronizado con el sonido
        float timer = 0f;

        while (timer < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeForce;
            float y = Random.Range(-1f, 1f) * shakeForce;

            playerCamera.transform.localPosition =
                originalCameraPosition + new Vector3(x, y, 0);

            timer += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.localPosition = originalCameraPosition;
        audioSource.Stop();

        // Vidrios rompiéndose
        if (glassBreakSound != null)
            audioSource.PlayOneShot(glassBreakSound, deathVolume);

        // Flash blanco
        yield return StartCoroutine(FlashWhite());

        Debug.Log("¡Jugador murió!");
    }

    IEnumerator FlashWhite()
    {
        if (whiteFlash == null)
            yield break;

        Color c = whiteFlash.color;

        // Aparece
        float t = 0f;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / 0.15f);
            whiteFlash.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        // Desaparece
        t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / 0.5f);
            whiteFlash.color = c;
            yield return null;
        }

        c.a = 0f;
        whiteFlash.color = c;
    }
}
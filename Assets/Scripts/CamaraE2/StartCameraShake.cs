using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class StartCameraShake : MonoBehaviour
{
    [Header("Configuración del Shake")]
    [Range(0f, 1f)]
    public float shakeForce = 0.2f;

    public float shakeDuration = 0.3f;
    public int repeatCount = 5;
    public float delayBetweenShakes = 0.2f;

    [Header("Sonido del Terremoto")]
    public AudioClip earthquakeSound;

    [Range(0f, 1f)]
    public float soundVolume = 1f;

    private Vector3 originalPosition;
    private AudioSource audioSource;

    void Start()
    {
        originalPosition = transform.localPosition;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true; // El sonido se mantiene mientras dura el shake.

        StartCoroutine(PlayShake());
    }

    IEnumerator PlayShake()
    {
        for (int i = 0; i < repeatCount; i++)
        {
            // Inicia el sonido al mismo tiempo que el shake.
            if (earthquakeSound != null)
            {
                audioSource.clip = earthquakeSound;
                audioSource.volume = soundVolume;
                audioSource.Play();
            }

            float timer = 0f;

            while (timer < shakeDuration)
            {
                float x = Random.Range(-1f, 1f) * shakeForce;
                float y = Random.Range(-1f, 1f) * shakeForce;

                transform.localPosition = originalPosition + new Vector3(x, y, 0f);

                timer += Time.deltaTime;
                yield return null;
            }

            // Termina el movimiento y el sonido al mismo tiempo.
            transform.localPosition = originalPosition;
            audioSource.Stop();

            if (i < repeatCount - 1)
                yield return new WaitForSeconds(delayBetweenShakes);
        }

        transform.localPosition = originalPosition;
    }
} 
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public string interactionName = "Examinar";

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    [Header("Depth of Field")]
    public Volume volume;

    [Header("Focus Settings")]
    public Camera playerCamera;

    [Header("Exit Settings")]
    public float exitDistance = 10f;

    [Header("Memory Effect")]
    public AudioSource memoryAudio;
    public float memoryDuration = 7f;

    private DepthOfField depthOfField;
    private Vignette vignette;

    private void Start()
    {
        if (volume != null)
        {
            volume.profile.TryGet(out depthOfField);
            volume.profile.TryGet(out vignette);

            if (depthOfField != null)
            {
                depthOfField.active = false;
            }

            if (vignette != null)
            {
                vignette.active = true;
                vignette.intensity.overrideState = true;
                vignette.intensity.value = 0f;
            }
        }

        if (memoryAudio != null)
        {
            memoryAudio.Stop();
        }
    }

    public void Interact()
    {
        Debug.Log("Interacción con: " + gameObject.name);

        // Si este objeto tiene diálogo
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue();
            return;
        }

        // Activar enfoque
        if (depthOfField != null)
        {
            depthOfField.active = true;

            depthOfField.focusDistance.overrideState = true;
            depthOfField.focusDistance.value = 8f;
        }

        // Activar viñeta
        if (vignette != null)
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = 0.35f;
        }

        // Activar audio del recuerdo
        if (memoryAudio != null)
        {
            StartCoroutine(MemoryEffect());
        }
    }

    private IEnumerator MemoryEffect()
    {
        Debug.Log("RECUERDO DEL CUADRO");

        memoryAudio.Play();

        // Mantener el recuerdo durante 7 segundos
        yield return new WaitForSeconds(memoryDuration);

        // Detener sonido
        memoryAudio.Stop();

        // Quitar viñeta
        if (vignette != null)
        {
            vignette.intensity.value = 0f;
        }

        // Quitar desenfoque
        if (depthOfField != null)
        {
            depthOfField.active = false;
        }

        Debug.Log("FIN DEL RECUERDO");
    }

    private void Update()
    {
        // Si este objeto tiene diálogo,
        // no controlar el Depth of Field automáticamente
        if (dialogueManager != null)
            return;

        if (depthOfField == null || playerCamera == null)
            return;

        float distance = Vector3.Distance(
            playerCamera.transform.position,
            transform.position
        );

        if (distance > exitDistance)
        {
            if (depthOfField != null)
            {
                depthOfField.active = false;
            }

            if (vignette != null)
            {
                vignette.intensity.value = 0f;
            }
        }
    }
}
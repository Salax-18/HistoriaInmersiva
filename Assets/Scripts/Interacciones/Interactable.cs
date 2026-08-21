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
    public AudioSource catherineAudio;
    public float memoryDuration = 7f;

    private DepthOfField depthOfField;
    private Vignette vignette;

    private bool memoryPlaying = false;

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

        if (catherineAudio != null)
        {
            catherineAudio.Stop();
        }
    }

    public void Interact()
    {
        Debug.Log("Interacción con: " + gameObject.name);

   

        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue();
            return;
        }


        if (memoryPlaying)
            return;

        // Activar desenfoque
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

        // Comenzar recuerdo
        if (memoryAudio != null)
        {
            StartCoroutine(MemoryEffect());
        }
    }

    private IEnumerator MemoryEffect()
    {
        memoryPlaying = true;

        Debug.Log("RECUERDO DEL CUADRO");


        if (memoryAudio != null)
        {
            memoryAudio.Play();
        }

    
        yield return new WaitForSeconds(memoryDuration);

       
        if (memoryAudio != null)
        {
            memoryAudio.Stop();
        }

        Debug.Log("TERMINAN LAS RISAS");

        //  Reproducir "Catherine..."
        if (catherineAudio != null)
        {
            Debug.Log("CATHERINE...");

            catherineAudio.Play();

            // Esperar hasta que termine la voz
            yield return new WaitWhile(() => catherineAudio.isPlaying);
        }

        
        if (vignette != null)
        {
            vignette.intensity.value = 0f;
        }

        if (depthOfField != null)
        {
            depthOfField.active = false;
        }

        Debug.Log("FIN DEL RECUERDO");

        memoryPlaying = false;
    }

    private void Update()
    {
   
        if (dialogueManager != null)
            return;

        if (depthOfField == null || playerCamera == null)
            return;

        float distance = Vector3.Distance(
            playerCamera.transform.position,
            transform.position
        );

        if (distance > exitDistance && !memoryPlaying)
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
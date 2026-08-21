using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    }

    public void Interact()
    {
        Debug.Log("Interacción con: " + gameObject.name);

      

        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue();
            return;
        }


        if (depthOfField != null)
        {
            depthOfField.active = true;

            depthOfField.focusDistance.overrideState = true;
            depthOfField.focusDistance.value = 8f;
        }

        if (vignette != null)
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = 0.35f;
        }
    }

    private void Update()
    {
        // Si este objeto es un terapeuta con diálogo,
        // no necesitamos controlar el Depth of Field.
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
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 3f;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    void Update()
    {
        if (Keyboard.current == null)
            return;

       
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("¡PRESIONÉ E!");

            
            if (dialogueManager != null && dialogueManager.dialogueActive)
            {
                dialogueManager.NextLine();
                return;
            }

            // Si no hay diálogo, continuar con las interacciones normales
            InteractWithObject();
        }
    }

    private void InteractWithObject()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                Debug.Log("Mirando: " + interactable.gameObject.name);

                Debug.Log("Intentando interactuar...");

                interactable.Interact();
            }
        }
    }
}
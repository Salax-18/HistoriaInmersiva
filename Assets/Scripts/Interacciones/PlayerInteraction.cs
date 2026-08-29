using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interacción")]
    public Camera playerCamera;
    public float interactionDistance = 3f;

    [Header("Diálogo")]
    public DialogueManager dialogueManager;

    [Header("Efecto de la Puerta")]
    public PlayerDeathEffect playerDeathEffect;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("¡PRESIONÉ E!");

            // Si hay un diálogo activo, continúa el diálogo.
            if (dialogueManager != null && dialogueManager.dialogueActive)
            {
                dialogueManager.NextLine();
                return;
            }

            // Si no hay diálogo, interactúa con el objeto.
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
            Debug.Log("Mirando: " + hit.collider.gameObject.name);

            // Si el objeto es la puerta, reproducir el efecto.
            if (hit.collider.CompareTag("Door"))
            {
                Debug.Log("Puerta interactuada.");

                if (playerDeathEffect != null)
                    playerDeathEffect.PlayEffect();

                return;
            }

            // Interacciones normales (NPC, objetos, etc.)
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                Debug.Log("Intentando interactuar...");
                interactable.Interact();
            }
        }
    }
}
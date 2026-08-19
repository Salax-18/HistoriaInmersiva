using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 3f;

    void Update()
    {
        // Prueba de teclado
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("¡PRESIONÉ E!");
        }

     
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

                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Debug.Log("Intentando interactuar...");
                    interactable.Interact();
                }
            }
        }
    }
}
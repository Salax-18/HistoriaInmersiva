using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SofaInteraction : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Sitting")]
    public Transform sitPoint;
    public Transform sittingCameraPoint;

    [Header("Settings")]
    public float interactionDistance = 2f;
    public float sittingDuration = 1f;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    private bool isSitting = false;

    private void Update()
    {
        if (isSitting)
            return;

        if (player == null || sitPoint == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            sitPoint.position
        );

        if (distance <= interactionDistance)
        {
            if (Keyboard.current != null &&
                Keyboard.current.sKey.wasPressedThisFrame)
            {
                PlayerMovement movement = player.GetComponent<PlayerMovement>();

                if (movement != null)
                {
                    movement.enabled = false;
                }

                StartCoroutine(SitDown());
            }
        }
    }

    private IEnumerator SitDown()
    {
        isSitting = true;

        Debug.Log("El jugador se está sentando");

        
        player.position = sitPoint.position;
        player.rotation = sitPoint.rotation;

        Debug.Log("El jugador está sentado");

        // Iniciar segundo diálogo
        if (dialogueManager != null)
        {
            dialogueManager.StartRegressionDialogue();
        }

        yield return null;
    }
}
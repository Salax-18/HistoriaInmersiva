using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Interactable : MonoBehaviour
{
    public string interactionName = "Examinar";

    [Header("Depth of Field")]
    public Volume volume;

    [Header("Focus Settings")]
    public Camera playerCamera;

    [Header("Exit Settings")]
    public float exitDistance = 10f;

    private DepthOfField depthOfField;

    private void Start()
    {
        if (volume != null)
        {
            volume.profile.TryGet(out depthOfField);

            // El efecto empieza apagado
            if (depthOfField != null)
            {
                depthOfField.active = false;
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Interacción con: " + gameObject.name);

        if (depthOfField != null)
        {
            // Activar el efecto al interactuar
            depthOfField.active = true;

            // Enfocar la fotografía
            depthOfField.focusDistance.overrideState = true;
            depthOfField.focusDistance.value = 8f;
        }
    }

    private void Update()
    {
        if (depthOfField == null || playerCamera == null)
            return;

        float distance = Vector3.Distance(
            playerCamera.transform.position,
            transform.position
        );

        // Al alejarse, volver a enfocar todo
        if (distance > exitDistance)
        {
            depthOfField.active = false;
        }
    }
}
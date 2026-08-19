using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string InteractionName = "Examinar";

    public void Interact()
    {
        Debug.Log($"Interactuando con {gameObject.name}");
    }
}

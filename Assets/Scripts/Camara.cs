using UnityEngine;
using UnityEngine.InputSystem;

public class Camara : MonoBehaviour
{
    [SerializeField] public float Sensibilidad = 100f;

    public Transform Player;

    private float RotacionX = 0f;

    void Update()
    {
        if (Mouse.current == null)
            return;

        float mouseX = Mouse.current.delta.x.ReadValue();
        float mouseY = Mouse.current.delta.y.ReadValue();

        mouseX *= Sensibilidad * Time.deltaTime;
        mouseY *= Sensibilidad * Time.deltaTime;

        // Mirar arriba y abajo
        RotacionX -= mouseY;

        RotacionX = Mathf.Clamp(RotacionX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(
            RotacionX,
            0f,
            0f
        );

   
        Player.Rotate(Vector3.up * mouseX);
    }
}
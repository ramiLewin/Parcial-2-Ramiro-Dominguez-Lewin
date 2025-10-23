using UnityEngine;
using UnityEngine.InputSystem;

public class camera : MonoBehaviour
{
[SerializeField] private Transform target;
    [SerializeField] private float distancia = 5f;
    [SerializeField] private float sensibilidad = 2f;
    [SerializeField] private float altura = 2f;

    private float rotacionX;
    private float rotacionY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Leer movimiento del mouse con el nuevo Input System
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        rotacionX += mouseDelta.x * sensibilidad * Time.deltaTime;
        rotacionY -= mouseDelta.y * sensibilidad * Time.deltaTime;
        rotacionY = Mathf.Clamp(rotacionY, -30f, 60f);

        Quaternion rotacion = Quaternion.Euler(rotacionY, rotacionX, 0f);
        Vector3 offset = rotacion * new Vector3(0, altura, -distancia);

        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * altura);
    }
}

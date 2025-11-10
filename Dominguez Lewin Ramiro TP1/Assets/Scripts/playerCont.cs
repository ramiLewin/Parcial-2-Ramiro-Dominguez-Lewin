using UnityEngine;
using UnityEngine.InputSystem; // nuevo namespace

[RequireComponent(typeof(Rigidbody))]

public class playerCont : MonoBehaviour
{ 
    [Header("ScriptableObject")]
    [SerializeField] public PlayerSO playerSO;

    
    [Header("Stats")]
    [SerializeField] public float vida = 100f;


    private Rigidbody rb;
    private Transform cam;
    private Vector2 moveInput;
    private Vector3 moveDir;

    private bool isCrouching = false;
    private Vector3 originalScale;
    private float originalSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        cam = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalScale = transform.localScale;
        originalSpeed = playerSO.moveSpeed;
    }

    private void Update()
    {
        // Detectar si se mantiene presionada la tecla C
        isCrouching = Keyboard.current.cKey.isPressed;

        // Cambiar escala y velocidad según el estado
        if (isCrouching)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
            playerSO.moveSpeed = originalSpeed * 0.5f;
        }
        else
        {
            transform.localScale = originalScale;
            playerSO.moveSpeed = originalSpeed;
        }
        // Leer input WASD
        moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;

        // Dirección del movimiento relativa a la cámara
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 direccion = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // Siempre rotar el personaje hacia donde mira la cámara (solo eje Y)
        Quaternion targetRotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, playerSO.rotationSpeed * Time.deltaTime);

        // Si hay movimiento, moverse en la dirección calculada
        moveDir = direccion;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir.normalized * playerSO.moveSpeed * Time.fixedDeltaTime);
    }

    public float GetVida() => vida;

}



using UnityEngine;
using UnityEngine.InputSystem; // nuevo namespace

[RequireComponent(typeof(Rigidbody))]

public class playerCont : MonoBehaviour
{ 
    [Header("ScriptableObject")]
    [SerializeField] private PlayerSO playerSO;
    
    /*[Header("Stats")]
    [SerializeField] private float vida = 100f;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;]*/

    private Rigidbody rb;
    private Transform cam;
    private Vector2 moveInput;
    private Vector3 moveDir;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        cam = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Leer input WASD
        moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;

        Vector3 direccion = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (direccion.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, PlayerSO.rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            moveDir = Vector3.zero;
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir.normalized * PlayerSO.moveSpeed * Time.fixedDeltaTime);
    }

    public float GetVida() => PlayerSO.vida;

}



using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class attack : MonoBehaviour
{
    
    [Header("ScriptableObject")]
    [SerializeField] public PlayerSO playerSO;
/*    [Header("Parámetros de ataque")]
    public float range = 20f;
    public float cadence = 2f; // disparos por segundo
    public int damage = 8;

    [Header("Visual del ataque")]
    public float lineDuration = 0.2f; // duración de la línea*/
    public Material lineMaterial; // opcional

    private float lastAttackTime = -999f;

    private int balasActuales = 15;         // balas que tiene el cargador
    private int balasMaximas = 15;          // capacidad del cargador --> pasar al SO

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ammoText;
 

    private void Start()
    {
        ActualizarUI();
        
    }
    private void Update()
    {
        // Recargar con R
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (balasActuales < balasMaximas)
            {
                balasActuales = balasMaximas;
                Debug.Log($"Recargado. Balas actuales: {balasActuales}/{balasMaximas}");
                //ActualizarUI; --> no funciona por algún motivo cuando está dentro de este if
                ActualizarUI();
                
            }
            else
            {
                Debug.Log("Cargador lleno. No se puede recargar.");
            }
        }

        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        // Verificar si hay balas disponibles
        if (balasActuales <= 0)
        {
            Debug.Log("Sin balas. Recarga con 'R'.");
            return;
        }

        // Cadencia
        float cooldown = (playerSO.cadence > 0f) ? (1f / playerSO.cadence) : Mathf.Infinity;
        if (Time.time - lastAttackTime < cooldown) return;
        lastAttackTime = Time.time;
        // Disminuir una bala al disparar
        balasActuales--;
        Debug.Log($"Disparo. Balas restantes: {balasActuales}/{balasMaximas}");
        ActualizarUI();
        Debug.Log("disparo");

        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 direction = transform.forward;

        RaycastHit hitInfo;
        Vector3 endPoint;

        if (Physics.Raycast(origin, direction, out hitInfo, playerSO.range))
        {
            endPoint = hitInfo.point;
            Debug.Log($"Impacto en {hitInfo.collider.name}");

            // Aplicar daño si golpea enemy
            enemy e = hitInfo.collider.GetComponentInParent<enemy>();
            if (e != null)
            {
                e.RecibirDaño(playerSO.damage);
                Debug.Log("Enemigo dañado");

            }
        }
        else
        {
            endPoint = origin + direction * playerSO.range;
            Debug.Log("No impactó a nada");
        }

        StartCoroutine(DrawLine(origin, endPoint));
    }

    private IEnumerator DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("AttackLine_Temp");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Unlit/Color")) { color = Color.green };
        lr.numCapVertices = 2;

        yield return new WaitForSeconds(playerSO.lineDuration);
        Destroy(lineObj);
    }
    private void ActualizarUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"balas: {balasActuales} / {balasMaximas}";
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // si usas TextMeshPro para el texto sobre la cabeza

[RequireComponent(typeof(Transform))]
public class enemy : MonoBehaviour
{
    [Header("ScriptableObject")]
    [SerializeField] public SoldierSO SoldierSO;

    public enum Estado { Idle, Persiguiendo, Muerto, Damaged }

    [Header("Stats")]
    [SerializeField] private int vida = 1;
    [SerializeField] private float moveSpeed = 3f;

    [Header("Visión")]
    // Si prefieres, puedes añadir variables aquí en lugar de usar el SO
    // [SerializeField] private float visionDistancia = 10f;
    // [SerializeField] private float visionAngulo = 90f;

    public Transform playerRef;
    private Transform player;
    private playerStamina playerStamina;
    private Estado estadoActual = Estado.Idle;

    /* --- NUEVO CÓDIGO --- */
    [Header("UI")]
    [SerializeField] private TextMeshPro estadoText;

    // Coroutine guardada para poder cancelarla si recibe daño repetido
    private Coroutine volverCoroutine = null;
    /* --- FIN NUEVO CÓDIGO --- */

    private void Start()
    {
        // Encontrar al player por tag
        player = GameObject.FindWithTag("Player")?.transform;

        if (player != null)
        {
            // Si quieres obtener un componente desde playerRef, asegúrate de asignarlo en el inspector
            if (playerRef != null)
                playerStamina = playerRef.GetComponent<playerStamina>();
            else
                playerStamina = player.GetComponent<playerStamina>();

            if (playerStamina == null)
            {
                Debug.LogWarning("[ENEMY] El objeto Player no tiene componente playerStamina.");
            }
        }
        else
        {
            Debug.LogError("[ENEMY] No se encontró ningún objeto con tag 'Player'.");
        }

        // Inicializar texto si existe
        if (estadoText != null)
        {
            estadoText.text = estadoActual.ToString();
        }
    }

    private void Update()
    {
        /*
        // Si está muerto, no hacer nada (pero mantengamos la seguridad de null checks)
        if (estadoActual == Estado.Muerto) return;
        if (player == null) return;

        bool playerVisible = DetectarJugador();

        if (playerVisible)
            CambiarEstado(Estado.Persiguiendo);
        else
            CambiarEstado(Estado.Idle);

        switch (estadoActual)
        {
            case Estado.Persiguiendo:
                PerseguirJugador();
                break;
            case Estado.Idle:
                // idle behavior
                break;
        }

        /* --- NUEVO CÓDIGO --- 
        // Hacer que el texto mire a la cámara (si todavía existe)
        if (estadoText != null && Camera.main != null)
        {
            estadoText.transform.LookAt(Camera.main.transform);
            estadoText.transform.Rotate(0f, 180f, 0f);
        }
        /* --- FIN NUEVO CÓDIGO --- */
            // Si está muerto, no hacer nada
    if (estadoActual == Estado.Muerto) return;

    if (player == null) return;

    // 🔹 Si está dañado, no detectar ni moverse (esperar a que vuelva al estado anterior)
    if (estadoActual == Estado.Damaged) return;

    // Detectar si el player está en el cono de visión
    bool playerVisible = DetectarJugador();
    if (playerVisible)
    {
        if (estadoActual != Estado.Persiguiendo)
            CambiarEstado(Estado.Persiguiendo);
    }

    // Ejecutar comportamiento según el estado actual
    switch (estadoActual)
    {
        case Estado.Persiguiendo:
            PerseguirJugador();
            break;
        case Estado.Idle:
            // No hacer nada cuando está idle
            break;
    }

    // Asegurarse de que el texto mire hacia la cámara
    if (estadoText != null && Camera.main != null)
    {
        estadoText.transform.LookAt(Camera.main.transform);
        estadoText.transform.Rotate(0, 180, 0);
    }
    }

    private bool DetectarJugador()
    {
        // Seguridad: si SoldierSO es null, sale false
        if (SoldierSO == null) return false;

        Vector3 direccionAlJugador = player.position - transform.position;
        Vector3 direccionXZ = new Vector3(direccionAlJugador.x, 0f, direccionAlJugador.z);
        float distancia = direccionXZ.magnitude;
        float angulo = Vector3.Angle(transform.forward, direccionXZ.normalized);

        bool dentroDelCono = distancia <= SoldierSO.visionDistancia && angulo <= SoldierSO.visionAngulo / 2f;

        if (dentroDelCono)
        {
            RaycastHit hitInfo;
            Vector3 origen = transform.position + Vector3.up * 1.5f;
            Vector3 direccion = (player.position + Vector3.up * 1f) - origen;

            if (Physics.Raycast(origen, direccion.normalized, out hitInfo, SoldierSO.visionDistancia))
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    Debug.DrawLine(origen, hitInfo.point, Color.green);
                    return true;
                }
                else
                {
                    Debug.DrawLine(origen, hitInfo.point, Color.red);
                    return false;
                }
            }
        }

        return false;
    }

    private void CambiarEstado(Estado nuevoEstado)
    {
        if (estadoActual == nuevoEstado) return;

        Estado estadoAnterior = estadoActual;
        estadoActual = nuevoEstado;

        Debug.Log($"[ENEMY] Estado cambiado de {estadoAnterior} → {estadoActual}");

        /* --- NUEVO CÓDIGO --- */
        if (estadoText != null)
        {
            // Aseguramos que el TMP siga existiendo
            estadoText.text = estadoActual.ToString();
        }
        /* --- FIN NUEVO CÓDIGO --- */

        if (playerStamina != null)
        {
            if (estadoActual == Estado.Persiguiendo)
                playerStamina.IniciarPersecucion();
            else
                playerStamina.DetenerPersecucion();
        }
    }

    private void PerseguirJugador()
    {
        Vector3 direccion = (player.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direccion.x, 0f, direccion.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        transform.position += direccion * moveSpeed * Time.deltaTime;
    }

    /* --- NUEVO CÓDIGO --- */
    public void RecibirDaño(int cantidad)
    {
        /* 
            if (estadoActual == Estado.Muerto) return;

            vida -= cantidad;

            // Guardar estado anterior para volver luego
            Estado estadoAnterior = estadoActual;

            // Si hay una coroutine pendiente que devolvería el estado anterior, la cancelamos
            if (volverCoroutine != null)
            {
                StopCoroutine(volverCoroutine);
                volverCoroutine = null;
            }

            // Cambiar inmediatamente a Damaged (esto actualiza el texto mediante CambiarEstado)
            CambiarEstado(Estado.Damaged);

            // Si murió por este golpe, manejar muerte de forma segura:
            if (vida <= 0)
            {
                CambiarEstado(Estado.Muerto);

                // Destruir el texto antes de destruir el enemigo para evitar MissingReferenceException
                if (estadoText != null)
                {
                    Destroy(estadoText.gameObject);
                    estadoText = null;
                }

                Destroy(gameObject);
                return;
            }

            // Si sigue vivo, lanzar coroutine para volver al estado anterior pasado un tiempo visible
            volverCoroutine = StartCoroutine(VolverAEstadoAnterior(estadoAnterior, 0.3f));
        
        */
            
        if (estadoActual == Estado.Muerto) return;

        vida -= cantidad;
        Estado estadoAnterior = estadoActual;

        // Cancelar cualquier coroutine pendiente
        if (volverCoroutine != null)
        {
            StopCoroutine(volverCoroutine);
            volverCoroutine = null;
        }

        // Cambiar estado a "Damaged"
        CambiarEstado(Estado.Damaged);

        // Forzar actualización visual inmediata
        if (estadoText != null)
        {
            estadoText.text = estadoActual.ToString();
            estadoText.color = Color.red; // cambia el color para resaltarlo
        }

        // Volver al estado anterior tras un pequeño delay
        volverCoroutine = StartCoroutine(VolverAEstadoAnterior(estadoAnterior, 0.5f));

        // Si muere, destruirlo
        if (vida <= 0)
        {
            CambiarEstado(Estado.Muerto);
            if (estadoText != null)
            {
                Destroy(estadoText.gameObject);
                estadoText = null;
            }
            Destroy(gameObject);
        }

    }

    private IEnumerator VolverAEstadoAnterior(Estado estadoAnterior, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (this == null) yield break;

        CambiarEstado(estadoAnterior);

        // Restaurar color original al texto
        if (estadoText != null)
        {
            estadoText.text = estadoActual.ToString();
            estadoText.color = Color.white;
        }

        volverCoroutine = null;

    }
    /* --- FIN NUEVO CÓDIGO --- */

    private void OnDrawGizmosSelected()
    {
        if (SoldierSO == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SoldierSO.visionDistancia);

        Vector3 derecha = Quaternion.Euler(0, SoldierSO.visionAngulo / 2f, 0) * transform.forward;
        Vector3 izquierda = Quaternion.Euler(0, -SoldierSO.visionAngulo / 2f, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + derecha * SoldierSO.visionDistancia);
        Gizmos.DrawLine(transform.position, transform.position + izquierda * SoldierSO.visionDistancia);
    }
}

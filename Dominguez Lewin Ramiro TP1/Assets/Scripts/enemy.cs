using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class enemy : MonoBehaviour
{
    [Header("ScriptableObject")]
    [SerializeField] public SoldierSO SoldierSO; 
    
    public enum Estado { Idle, Persiguiendo, Muerto, Damaged}

    [Header("Stats")]
    [SerializeField] private int vida = 1;
    [SerializeField] private float moveSpeed = 3f;
    
    [Header("Visión")]


    public Transform playerRef;
    private Transform player;
    private playerStamina playerStamina;
    private Estado estadoActual = Estado.Idle;

    [Header("UI")]
    [SerializeField] private TextMeshPro estadoText;

    private void Start()
    {
        // Encontrar al player
        player = GameObject.FindWithTag("Player")?.transform;

        /*if (player != null)
        {
            playerStamina = player.GetComponent<stamina>();
        }*/
        if (player != null)
        {
            playerStamina = playerRef.GetComponent<playerStamina>();

            if (playerStamina == null)
            {
                Debug.LogWarning($"[ENEMY] El objeto con tag 'Player' no tiene el componente 'stamina'.", player.gameObject);
            }
        }
        else
        {
            Debug.LogError("[ENEMY] No se encontró ningún objeto con tag 'Player'.");
        }
        if (estadoText != null)
        {
            estadoText.text = estadoActual.ToString();
        }
    }

    private void Update()
    {
        // Si está muerto, no hacer nada
        if (estadoActual == Estado.Muerto) return;

        if (player == null) return;

        // Detectar si el player está en el cono de visión
        bool playerVisible = DetectarJugador();

        // Cambiar estado según si ve al player
        if (playerVisible)
        {
            CambiarEstado(Estado.Persiguiendo);
        }
        else
        {
            CambiarEstado(Estado.Idle);
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
        // Calcular distancia y ángulo en el plano XZ (ignorando altura)
        Vector3 direccionAlJugador = player.position - transform.position;
        Vector3 direccionXZ = new Vector3(direccionAlJugador.x, 0f, direccionAlJugador.z);
        float distancia = direccionXZ.magnitude;
        float angulo = Vector3.Angle(transform.forward, direccionXZ.normalized);

        bool dentroDelCono = distancia <= SoldierSO.visionDistancia && angulo <= SoldierSO.visionAngulo / 2f;
 
        if (dentroDelCono)
        {
            RaycastHit hitInfo;

            // Hacemos un raycast hacia el jugador
            if (Physics.Raycast(transform.position + Vector3.up * 1.5f, direccionAlJugador.normalized, out hitInfo, SoldierSO.visionDistancia))
            {
                // Si lo primero que toca el rayo es el jugador, entonces lo ve
                if (hitInfo.collider.CompareTag("Player"))
                {
                    return true;
                }
                else
                {
                    // Si golpea otra cosa primero (una pared, etc.), la visión está bloqueada
                    Debug.DrawLine(transform.position + Vector3.up * 1.5f, hitInfo.point, Color.red);
                    return false;
                }
            }
        }
        // Está dentro del cono de visión?
        return distancia <= SoldierSO.visionDistancia && angulo <= SoldierSO.visionAngulo / 2f;
    }

    /*private void CambiarEstado(Estado nuevoEstado)
    {
        // Solo cambiar si es diferente
        if (estadoActual == nuevoEstado) return;

        estadoActual = nuevoEstado;
        Debug.Log("el estado del enemigo es: "+estadoActual);

        // Notificar al player según el nuevo estado
        if (playerStamina != null)
        {
            if (estadoActual == Estado.Persiguiendo)
            {
                playerStamina.IniciarPersecucion();
            }
            else
            {
                playerStamina.DetenerPersecucion();
            }
        }
    }*/
    private void CambiarEstado(Estado nuevoEstado)
{
    // Solo cambiar si es diferente
    if (estadoActual == nuevoEstado) return;

    Estado estadoAnterior = estadoActual;
    estadoActual = nuevoEstado;

    Debug.Log($"[ENEMY] Estado cambiado de {estadoAnterior} → {estadoActual}");
    
    if (estadoText != null)
        {
            estadoText.text = estadoActual.ToString();
        }
    // Notificar al player según el nuevo estado
    if (playerStamina != null)
    {
        if (estadoActual == Estado.Persiguiendo)
        {
            playerStamina.IniciarPersecucion();
        }
        else
        {
            playerStamina.DetenerPersecucion();
        }
    }
}


    private void PerseguirJugador()
    {
        // Dirección hacia el player
        Vector3 direccion = (player.position - transform.position).normalized;

        // Rotar suavemente hacia el player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direccion.x, 0, direccion.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Moverse hacia el player
        transform.position += direccion * moveSpeed * Time.deltaTime;
    }

    public void RecibirDaño(int cantidad)
    {
        if (estadoActual == Estado.Muerto) return;

        vida -= cantidad;
        // Guardar el estado anterior
        Estado estadoAnterior = estadoActual;

        // Cambiar temporalmente a "Damaged"
        CambiarEstado(Estado.Damaged);

        // Volver al estado anterior después de un instante
        StartCoroutine(VolverAEstadoAnterior(estadoAnterior));
        if (vida <= 0)
        {
            CambiarEstado(Estado.Muerto);
            Destroy(gameObject);
        }
    }
    
    private IEnumerator VolverAEstadoAnterior(Estado estadoAnterior)
    {
        // Pequeña pausa visual (puedes ajustar el tiempo)
        yield return new WaitForSeconds(1f);

        CambiarEstado(estadoAnterior);
    }
    // Dibujar el cono de visión en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SoldierSO.visionDistancia);

        Vector3 derecha = Quaternion.Euler(0, SoldierSO.visionAngulo / 2, 0) * transform.forward;
        Vector3 izquierda = Quaternion.Euler(0, -SoldierSO.visionAngulo / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + derecha * SoldierSO.visionDistancia);
        Gizmos.DrawLine(transform.position, transform.position + izquierda * SoldierSO.visionDistancia);
    }

}

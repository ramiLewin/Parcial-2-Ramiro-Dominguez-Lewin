using UnityEngine;

public class enemyv2 : MonoBehaviour
{
   /* public enum Estado { Idle, Persiguiendo, Muerto }

    [Header("Stats")]
    [SerializeField] private int vida = 1;
    [SerializeField] private float moveSpeed = 3f;

    [Header("Visión")]
    [SerializeField] private float visionDistancia = 10f;
    [SerializeField] private float visionAngulo = 90f;

    private Transform player;
    private stamina playerStamina;
    private Estado estadoActual = Estado.Idle;

    private void Start()
    {
        // Encontrar al player
        player = GameObject.FindWithTag("Player")?.transform;

        if (player != null)
        {
            playerStamina = player.GetComponent<stamina>();
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
    }

    private bool DetectarJugador()
    {
        // Calcular distancia y ángulo en el plano XZ (ignorando altura)
        Vector3 direccionAlJugador = player.position - transform.position;
        Vector3 direccionXZ = new Vector3(direccionAlJugador.x, 0f, direccionAlJugador.z);
        float distancia = direccionXZ.magnitude;
        float angulo = Vector3.Angle(transform.forward, direccionXZ.normalized);

        // Está dentro del cono de visión?
        return distancia <= visionDistancia && angulo <= visionAngulo / 2f;
    }

    private void CambiarEstado(Estado nuevoEstado)
    {
        // Solo cambiar si es diferente
        if (estadoActual == nuevoEstado) return;

        estadoActual = nuevoEstado;

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

        if (vida <= 0)
        {
            CambiarEstado(Estado.Muerto);
            Destroy(gameObject);
        }
    }

    // Dibujar el cono de visión en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionDistancia);

        Vector3 derecha = Quaternion.Euler(0, visionAngulo / 2, 0) * transform.forward;
        Vector3 izquierda = Quaternion.Euler(0, -visionAngulo / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + derecha * visionDistancia);
        Gizmos.DrawLine(transform.position, transform.position + izquierda * visionDistancia);
    }*/
}

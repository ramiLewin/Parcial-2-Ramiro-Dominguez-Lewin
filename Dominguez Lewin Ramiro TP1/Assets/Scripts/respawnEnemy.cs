using UnityEngine;
using UnityEngine.InputSystem;

public class respawnEnemy : MonoBehaviour
{
[Tooltip("Prefab del enemigo (arrastrar desde Project, NO desde la escena).")]
    public GameObject enemyPrefab;

    [Tooltip("Radio (m) para buscar un enemy ya presente en la posición de spawn.")]
    public float detectRadius = 0.6f;

    private GameObject currentEnemy;
    private Vector3 spawnPos;
    private Quaternion spawnRot;

    private void Start()
    {
        spawnPos = transform.position;
        spawnRot = transform.rotation;

        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyRespawner: No se asignó enemyPrefab en el inspector.");
            return;
        }

        // Buscar si ya hay un enemy en la posición del spawn (evita duplicados)
        Collider[] cols = Physics.OverlapSphere(spawnPos, detectRadius);
        foreach (var c in cols)
        {
            var e = c.GetComponentInParent<enemy>();
            if (e != null)
            {
                currentEnemy = e.gameObject;
                Debug.Log($"EnemyRespawner: Encontró enemy existente '{currentEnemy.name}' en escena; no se instancia uno nuevo.");
                return;
            }
        }

        // Si no hay ninguno, instanciamos el prefab
        SpawnEnemies();
    }

    private void Update()
    {
        // Respawn manual con F3
        if (Keyboard.current != null && Keyboard.current.f3Key.wasPressedThisFrame)
        {
            RespawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        if (enemyPrefab == null) return;

        currentEnemy = Instantiate(enemyPrefab, spawnPos, spawnRot);
        currentEnemy.name = enemyPrefab.name; // opcional: quitar "(Clone)" para limpieza
        Debug.Log($"EnemyRespawner: Spawned '{currentEnemy.name}' en {spawnPos}.");
    }

    public void RespawnEnemies()
    {
        // Si hay un enemy actual (vivo/inactivo), lo destruimos primero para garantizar limpieza
        if (currentEnemy != null)
        {
            Debug.Log("EnemyRespawner: Destruyendo enemy actual antes de respawnear.");
            Destroy(currentEnemy);
            currentEnemy = null;
        }

        // Instanciamos uno nuevo
        SpawnEnemies();
        Debug.Log("EnemyRespawner: Respawn completo.");
    }

    // Util: para debugging en escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}

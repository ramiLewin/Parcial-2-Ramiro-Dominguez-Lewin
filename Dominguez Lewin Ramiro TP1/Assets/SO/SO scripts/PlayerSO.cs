using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{

    [Header("Stats")]
    public float vida;

    [Header("Movimiento")]
    public float moveSpeed;
    public float rotationSpeed;

    [Header("Stamina")]
    public float estaminaMax = 10f;
    public float regeneracionRate = 1f;  // Cuánto sube por segundo
    public float drainRate = 2f;  // Cuánto baja por segundo cuando te persiguen

    [Header("Parámetros de ataque")]
    public float range = 20f;
    public float cadence = 2f; // disparos por segundo
    public int damage = 8;

    [Header("Visual del ataque")]
    public float lineDuration = 0.2f; // duración de la línea
}

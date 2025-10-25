using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{

    [Header("Stats")]
    [SerializeField] public float vida;

    [Header("Movimiento")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotationSpeed;
}

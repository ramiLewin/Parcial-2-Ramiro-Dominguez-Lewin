using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{

    [Header("Stats")]
    [SerializeField] private float vida;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
}

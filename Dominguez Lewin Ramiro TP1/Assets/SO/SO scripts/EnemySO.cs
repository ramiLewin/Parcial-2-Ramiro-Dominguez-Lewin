using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Visión")]
    [SerializeField] private float visionDistancia = 10f;
    [SerializeField] private float visionAngulo = 90f;
}

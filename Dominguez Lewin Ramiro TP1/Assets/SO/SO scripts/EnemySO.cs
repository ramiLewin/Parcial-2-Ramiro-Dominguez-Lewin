using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Visión")]
    [SerializeField] public float visionDistancia = 10f;
    [SerializeField] public float visionAngulo = 90f;
}

using UnityEngine;

[CreateAssetMenu(fileName = "SoldierSO", menuName = "Scriptable Objects/SoldierSO")]
public class SoldierSO : ScriptableObject
{
    [Header("Visión")]
    [SerializeField] public float visionDistancia = 10f;
    [SerializeField] public float visionAngulo = 90f;
}

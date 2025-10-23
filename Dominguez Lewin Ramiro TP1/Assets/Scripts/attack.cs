using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class attack : MonoBehaviour
{
    [Header("Parámetros de ataque")]
    public float range = 20f;
    public float cadence = 2f; // disparos por segundo
    public int damage = 8;

    [Header("Visual")]
    public float lineDuration = 0.2f; // duración de la línea
    public Material lineMaterial; // opcional

    private float lastAttackTime = -999f;

    private void Update()
    {
        if (Mouse.current == null) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        // Cadencia
        float cooldown = (cadence > 0f) ? (1f / cadence) : Mathf.Infinity;
        if (Time.time - lastAttackTime < cooldown) return;
        lastAttackTime = Time.time;

        Debug.Log("disparo");

        Vector3 origin = transform.position + Vector3.up * 1.5f; // un poco más alto (como la altura del arma)
        Vector3 direction = transform.forward;

        RaycastHit hitInfo;
        Vector3 endPoint;

        if (Physics.Raycast(origin, direction, out hitInfo, range))
        {
            endPoint = hitInfo.point;
            Debug.Log($"Impacto en {hitInfo.collider.name}");

            // Aplicar daño si golpea enemy
            enemy e = hitInfo.collider.GetComponentInParent<enemy>();
            if (e != null)
            {
                e.RecibirDaño(damage);
                Debug.Log("Enemigo dañado");
            }
        }
        else
        {
            endPoint = origin + direction * range;
            Debug.Log("No impactó a nada");
        }

        StartCoroutine(DrawLine(origin, endPoint));
    }

    private IEnumerator DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("AttackLine_Temp");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Unlit/Color")) { color = Color.green };
        lr.numCapVertices = 2;

        yield return new WaitForSeconds(lineDuration);
        Destroy(lineObj);
    }
}

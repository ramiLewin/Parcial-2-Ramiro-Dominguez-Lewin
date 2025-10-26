using UnityEngine;

public class playerStamina : MonoBehaviour
{
  
   /* [Header("Estamina")]
    [SerializeField] private float estamina = 10f;
    [SerializeField] private float estaminaMax = 10f;
    [SerializeField] private float estaminaMin = 0f;

    // Flag controlado por el enemy
    private bool siendoPerseguido;

    private void Update()
    {
        ActualizarEstamina();
        
        Debug.Log("la stamina sube?: "+ siendoPerseguido);
    }

    private void ActualizarEstamina()
    {
        if (siendoPerseguido==true)        // hay prblemas con la estamina, nunca baja, parece que "siendoPerseguido" es siempre false
        {
            estamina -= 1f * Time.deltaTime;
            Debug.Log("lo persiguen");
        }
        else
        {
            estamina += 1f * Time.deltaTime;
                Debug.Log("no lo persiguen");
        }

    estamina = Mathf.Clamp(estamina, estaminaMin, estaminaMax);

    }

    // Método que llamará el enemy cuando cambie su estado
    public void SetSiendoPerseguido(bool valor)
    {
        siendoPerseguido = valor;
        Debug.Log($"[Player] Siendo perseguido = {siendoPerseguido}");
    }

    // Métodos utilitarios para leer stats
    public float GetEstamina() => estamina;
*/

    [Header("ScriptableObject")]
    [SerializeField] public PlayerSO playerSO;

    [Header("Estamina")]
    [SerializeField] private float estamina = 10f;
    /*
    [SerializeField] private float estaminaMax = 10f;
    [SerializeField] private float regeneracionRate = 1f;  // Cuánto sube por segundo
    [SerializeField] private float drainRate = 2f;  // Cuánto baja por segundo cuando te persiguen
*/

    private bool siendoPerseguido = false;

    private void Update()
    {
        ActualizarEstamina();
    }

    private void ActualizarEstamina()
    {
        if (siendoPerseguido)
        {
            // Bajar estamina cuando te persiguen
            estamina -= playerSO.drainRate * Time.deltaTime;
        }
        else
        {
            // Regenerar estamina cuando estás seguro
            estamina += playerSO.regeneracionRate * Time.deltaTime;
        }

        // Mantener la estamina entre 0 y el máximo
        estamina = Mathf.Clamp(estamina, 0f, playerSO.estaminaMax);
    }

    // El enemigo llama esto cuando empieza a perseguir
    public void IniciarPersecucion()
    {
        siendoPerseguido = true;
        Debug.Log("siendoPerseguido es: "+siendoPerseguido);
    }

    // El enemigo llama esto cuando deja de perseguir
    public void DetenerPersecucion()
    {
        siendoPerseguido = false;
        Debug.Log("siendoPerseguido es: "+siendoPerseguido);
    }

    // Para leer la estamina actual
    public float GetEstamina() => estamina;

    // Para saber si está siendo perseguido (útil para UI o efectos)
    public bool EstasiendoPerseguido() => siendoPerseguido;
}


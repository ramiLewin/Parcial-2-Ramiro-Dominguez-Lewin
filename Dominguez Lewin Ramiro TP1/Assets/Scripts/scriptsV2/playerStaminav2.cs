using UnityEngine;

public class playerStaminav2 : MonoBehaviour
{
    /*[Header("Estamina")]
    [SerializeField] private float estamina = 10f;
    [SerializeField] private float estaminaMax = 10f;
    [SerializeField] private float regeneracionRate = 1f;  // Cuánto sube por segundo
    [SerializeField] private float drainRate = 2f;  // Cuánto baja por segundo cuando te persiguen

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
            estamina -= drainRate * Time.deltaTime;
        }
        else
        {
            // Regenerar estamina cuando estás seguro
            estamina += regeneracionRate * Time.deltaTime;
        }

        // Mantener la estamina entre 0 y el máximo
        estamina = Mathf.Clamp(estamina, 0f, estaminaMax);
    }

    // El enemigo llama esto cuando empieza a perseguir
    public void IniciarPersecucion()
    {
        siendoPerseguido = true;
    }

    // El enemigo llama esto cuando deja de perseguir
    public void DetenerPersecucion()
    {
        siendoPerseguido = false;
    }

    // Para leer la estamina actual
    public float GetEstamina() => estamina;

    // Para saber si está siendo perseguido (útil para UI o efectos)
    public bool EstasiendoPerseguido() => siendoPerseguido;*/
}

using UnityEngine;
using TMPro;   

public class UIPlayerVida : MonoBehaviour
{
    public playerCont player;          // Referencia al script del jugador
    public TextMeshProUGUI vidaText;   // Texto TMP en la UI

    private void Update()
    {
        if (player != null && vidaText != null)
        {
            vidaText.text = "Vida: " + player.GetVida().ToString("0");
        }
    }
}
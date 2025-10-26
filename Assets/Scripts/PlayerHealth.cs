using UnityEngine;
using TMPro; // solo si usas TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de vida")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;

        UpdateUI();

        if (currentHealth <= 0)
        {
            Debug.Log("💀 El jugador ha muerto");
            // Aquí podrías reiniciar el nivel o mostrar una pantalla de derrota
        }
    }

    private void UpdateUI()
    {
        if (healthText != null)
            healthText.text = $"Vida: {currentHealth}";
    }
}

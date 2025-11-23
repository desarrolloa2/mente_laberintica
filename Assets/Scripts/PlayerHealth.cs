using UnityEngine;
using TMPro;
using UnityEngine.UI; // para usar Image
using System.Collections; // para corrutinas
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de vida")]
    public int maxHealth = 100;
    public int currentHealth;


    [Header("UI")]
    public TextMeshProUGUI healthText;
    public Image damageImage;            // imagen de daño
    public float fadeDuration = 3f;      // duración del desvanecimiento
    public Color damageColor = new Color(1f, 0f, 0f, 0.4f); // color del flash

    private Coroutine damageCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        if (damageImage != null)
            damageImage.color = new Color(damageColor.r, damageColor.g, damageColor.b, 0f);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
            currentHealth = 0;

        UpdateUI();

        // Mostrar el efecto de daño
        if (damageImage != null)
        {
            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);

            damageCoroutine = StartCoroutine(ShowDamageEffect());
        }

        if (currentHealth <= 0)
        {
            Debug.Log("💀 El jugador ha muerto");
            SceneManager.LoadScene("GameOver"); // ← Nombre exacto de tu escena
        }
    }

    private IEnumerator ShowDamageEffect()
    {
        // Mostrar imagen de daño
        damageImage.color = damageColor;

        // Desvanecer gradualmente
        float elapsed = 0f;
        Color startColor = damageColor;
        Color endColor = new Color(damageColor.r, damageColor.g, damageColor.b, 0f);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            damageImage.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            yield return null;
        }

        damageImage.color = endColor;
        damageCoroutine = null;
    }

    private void UpdateUI()
    {
        if (healthText != null)
            healthText.text = $"Vida: {currentHealth}";
    }
}

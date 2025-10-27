using UnityEngine;
using TMPro; // solo si usas TextMeshPro

public class PlayerInventory : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI heartText;
    public TextMeshProUGUI clockText;

    [Header("Puerta")]
    public GameObject door; // puerta a destruir
    public int heartsRequired = 5;
    public int clocksRequired = 5;
    private bool doorOpened = false;

    private int hearts = 0;
    private int clocks = 0;

    void Start()
    {
        UpdateUI();
    }

    public void AddItem(CollectibleItem.ItemType type)
    {
        switch (type)
        {
            case CollectibleItem.ItemType.Heart:
                hearts++;
                break;
            case CollectibleItem.ItemType.Clock:
                clocks++;
                break;
        }

        UpdateUI();
        CheckDoorCondition();
    }

    private void CheckDoorCondition()
    {
        if (!doorOpened && hearts >= heartsRequired && clocks >= clocksRequired)
        {
            doorOpened = true;

            if (door != null)
            {
                // Puedes elegir entre ocultarla o destruirla
                // door.SetActive(false);
                Destroy(door);
                Debug.Log("🚪 ¡Puerta abierta! Has recolectado todos los ítems.");
            }
        }
    }

    private void UpdateUI()
    {
        if (heartText != null)
            heartText.text = $"= {hearts}";
        if (clockText != null)
            clockText.text = $"= {clocks}";
    }
}

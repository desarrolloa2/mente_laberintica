using UnityEngine;
using TMPro; // solo si usas TextMeshPro

public class PlayerInventory : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI heartText;
    public TextMeshProUGUI clockText;

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
    }

    private void UpdateUI()
    {
        if (heartText != null)
            heartText.text = $"= {hearts}";
        if (clockText != null)
            clockText.text = $"= {clocks}";
    }
}

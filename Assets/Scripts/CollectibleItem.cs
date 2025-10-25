using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType { Heart, Clock }
    public ItemType itemType = ItemType.Heart; // seleccionable desde el Inspector

    public float rotationSpeed = 90f;
    public string playerTag = "Player";

    void Update()
    {
        // Rotar todo el tiempo
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemType);
            }

            Destroy(gameObject);
        }
    }

}

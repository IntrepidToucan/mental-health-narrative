using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    [Header("Item Requirements")]
    [SerializeField] private bool requiresItems = false;  // Toggle to enable/disable item requirements
    [SerializeField] private List<Item> requiredItems;    // List of required items (ScriptableObjects)

    private bool areItemsPlaced = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (requiresItems && !areItemsPlaced)
            {
                if (CheckForRequiredItems())
                {
                    areItemsPlaced = true;
                    // Remove items from inventory
                    RemoveRequiredItems();
                    Debug.Log("All required items placed. The device is now armed.");
                }
                else
                {
                    Debug.Log("Required items are missing. The device cannot be armed.");
                    return;
                }
            }

            // Proceed to trigger the explosion if items are placed or no items are required
            TriggerExplosion();
        }
    }

    private bool CheckForRequiredItems()
    {
        foreach (var item in requiredItems)
        {
            if (!Inventory.Instance.HasItem(item.itemId))
            {
                return false;
            }
        }
        return true;
    }

    private void RemoveRequiredItems()
    {
        foreach (var item in requiredItems)
        {
            Inventory.Instance.RemoveItem(item.itemName);
        }
    }

    private void TriggerExplosion()
    {
        // Logic to trigger the explosion
        Debug.Log("Explosion triggered!");
        // Add your explosion logic here
    }
}

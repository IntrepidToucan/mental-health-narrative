using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Inventory : PersistedSingleton<Inventory>
{
    public List<Item> items = new(); // Holds the inventory items

    // Delegate and event for notifying when the inventory is updated
    public delegate void InventoryUpdate();
    public static event InventoryUpdate OnInventoryUpdated;

    // Method to add items to the inventory
    public void AddItem(Item item)
    {
        items.Add(item);
        OnInventoryUpdated?.Invoke();  // Trigger the event
        Debug.Log($"Added {item.itemName} to inventory");
    }

    // Method to check if an item is in the inventory
    public bool HasItem(string id)
    {
        return items.Exists(item => item.itemId == id);
    }

    // Method to remove items from the inventory
    public void RemoveItem(string id)
    {
        var item = items.Find(i => i.itemId == id);
        
        if (item != null)
        {
            items.Remove(item);
            OnInventoryUpdated?.Invoke();
            Debug.Log($"Removed {item.itemName} from inventory");
        }
        else
        {
            Debug.LogError($"Item {id} not found in inventory");
        }
    }
}

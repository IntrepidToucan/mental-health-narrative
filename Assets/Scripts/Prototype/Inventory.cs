using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(); // Holds the inventory items

    // Delegate and event for notifying when the inventory is updated
    public delegate void InventoryUpdate();
    public static event InventoryUpdate OnInventoryUpdated;

    public static Inventory Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Ensure there is only one instance of Inventory
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make the inventory persistent across scenes
        }
    }

    // Method to add items to the inventory
    public void AddItem(Item item)
    {
        items.Add(item);
        OnInventoryUpdated?.Invoke();  // Trigger the event
        Debug.Log($"Added {item.itemName} to inventory");
    }

    // Method to check if an item is in the inventory
    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }
}

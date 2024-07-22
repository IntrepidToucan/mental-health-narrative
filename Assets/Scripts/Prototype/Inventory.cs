using System;
using System.Collections.Generic;
using Characters.NPCs;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

public class Inventory : PersistedSingleton<Inventory>
{
    public static bool IsDefaultItemId(ItemId itemId) => itemId == ItemId.None;
    public static string GetNpcGoalItemIdString(NpcData npcData) =>
        $"{Enum.GetName(typeof(NpcId), npcData.NpcId)}BloodSample";
    
    public static ItemId TryParseItemId(string idString)
    {
        if (!Enum.TryParse(idString, out ItemId result))
        {
            Debug.LogError($"Invalid item ID string {idString}");
        }

        return result;
    }
    
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

        var notificationData = Instantiate(SceneManager.Instance.NotificationData_ItemAcquired);
        notificationData.text = string.Format(notificationData.text, item.itemName);
        EventManager.TriggerNotification(notificationData);
    }
    
    public bool HasItem(ItemId itemId) => items.Exists(item => item.itemId == itemId);

    // Method to check if an item is in the inventory
    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }

    // Method to remove items from the inventory
    public void RemoveItem(string itemName)
    {
        var item = items.Find(i => i.itemName == itemName);
        
        if (item != null)
        {
            items.Remove(item);
            OnInventoryUpdated?.Invoke();
            Debug.Log($"Removed {item.itemName} from inventory");
        }
        else
        {
            Debug.LogError($"Item {itemName} not found in inventory");
        }
    }
}

using System.Collections.Generic;
using TMPro; // Use TextMeshPro namespace if using TextMeshPro
using UnityEngine;
using UnityEngine.UI; // Use this if using regular Text component

public class InventoryUIP : MonoBehaviour
{
    // Reference to the UI text element. Use TMP_Text for TextMeshPro or Text for regular UI text
    [SerializeField] private TMP_Text inventoryText; // Replace TMP_Text with Text if not using TextMeshPro

    private void OnEnable()
    {
        Inventory.OnInventoryUpdated += UpdateInventoryUI;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryUpdated -= UpdateInventoryUI;
    }

    private void Start()
    {
        UpdateInventoryUI(); // Initial update
    }

    private void UpdateInventoryUI()
    {
        Inventory playerInventory = Inventory.Instance;
        if (playerInventory != null)
        {
            List<string> itemNames = new List<string>();
            foreach (Item item in playerInventory.items)
            {
                itemNames.Add(item.itemName);
            }
            inventoryText.text = string.Join("\n", itemNames);
        }
        else
        {
            inventoryText.text = "Inventory is empty";
        }
    }
}

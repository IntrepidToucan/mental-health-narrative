using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryPage inventoryUI;
    private InputAction _inventoryAction;
    public int inventorySize = 10;

    private void Start()
    {
        // Initialize the inventory UI
        inventoryUI.InitializeInventoryUI(inventorySize);

        // Find and bind the inventory action
        _inventoryAction = InputSystem.actions.FindAction("Inventory");

        if (_inventoryAction == null)
        {
            Debug.LogError("Inventory action not found in the Input Action asset.");
            return;
        }

        _inventoryAction.Enable();
        _inventoryAction.performed += ToggleInventory;
    }

    private void OnDestroy()
    {
        if (_inventoryAction != null)
        {
            _inventoryAction.performed -= ToggleInventory;
        }
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        if (!inventoryUI.isActiveAndEnabled)
        {
            inventoryUI.Show();
        }
        else
        {
            inventoryUI.Hide();
        }
    }
}

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
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on the GameObject.");
            return;
        }

        _inventoryAction = playerInput.actions["Inventory"];

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

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        Debug.Log($"Inventory active before toggle: {inventoryUI.gameObject.activeSelf}");
        if (!inventoryUI.gameObject.activeSelf)
        {
            inventoryUI.Show();
        }
        else
        {
            inventoryUI.Hide();
        }
        Debug.Log($"Inventory active after toggle: {inventoryUI.gameObject.activeSelf}");
    }

}
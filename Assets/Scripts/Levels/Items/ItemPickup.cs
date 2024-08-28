using Interaction;
using UnityEngine;
using System;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item itemData; // ScriptableObject reference for the item details
    public event Action OnItemPickedUp; // Event triggered when the item is picked up

    public void Interact()
    {
        // Assuming the player has an 'Inventory' component where items can be added
        Inventory.Instance.AddItem(itemData);
        Debug.Log($"Item picked up: {itemData.itemName}");

        // Trigger the item picked up event
        OnItemPickedUp?.Invoke();

        // Destroy the item GameObject after it is picked up
        Destroy(gameObject);
    }
    
    public bool CanInteract()
    {
        return true;
    }

    // This method returns interaction prompt data
    public IInteractable.InteractionData? GetInteractionData()
    {
        // Return a new instance of InteractionData with the prompt message
        return new IInteractable.InteractionData($"Press 'E' to pick up {itemData.itemName}.");
    }
}

using Interaction;
using UnityEngine;

public class ShovelInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject dirtPile;  // Reference to the dirt pile
    [SerializeField] private GameObject hiddenFlower;  // The flower hidden under the dirt pile
    [SerializeField] private string shovelItemName = "Shovel";  // The name of the shovel item in the inventory

    private bool isUsed = false;

    public bool CanInteract()
    {
        // Check if the player has the shovel in the inventory and the dirt hasn't been dug up yet
        return !isUsed && Inventory.Instance.HasItem(shovelItemName);
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        // Show interaction prompt only if the shovel is in the inventory
        if (Inventory.Instance.HasItem(shovelItemName))
        {
            return new IInteractable.InteractionData("Press 'E' to use the shovel");
        }
        return null;  // No interaction prompt if shovel is not in inventory
    }

    public void Interact()
    {
        if (!isUsed && Inventory.Instance.HasItem(shovelItemName))
        {
            isUsed = true;
            RemoveDirtPile();
            RemoveShovelFromInventory();  // Remove the shovel after using it
        }
    }

    private void RemoveDirtPile()
    {
        // Remove or disable the dirt pile
        if (dirtPile != null)
        {
            Destroy(dirtPile);  // You could also use SetActive(false) if you prefer
        }

        // Show the hidden flower
        if (hiddenFlower != null)
        {
            hiddenFlower.SetActive(true);
        }

        Debug.Log("Dirt pile removed, hidden flower revealed.");
    }

    private void RemoveShovelFromInventory()
    {
        // Remove the shovel from the inventory
        Inventory.Instance.RemoveItem(shovelItemName);
        Debug.Log("Shovel removed from inventory.");
    }
}

using Characters.Player;
using Interaction;
using UnityEngine;

public class PortalDoor : MonoBehaviour, IInteractable
{
    public string requiredItem; // Item required to use the portal
    public PortalDoor pairedDoor; // Reference to the paired door
    public bool requiresItem = true; // Can be toggled in the Unity Editor
    public void Interact()
    {
        if (requiresItem && !Inventory.Instance.HasItem(requiredItem))
        {
            Debug.Log("You need a special key to use this portal.");
            return;
        }

        if (pairedDoor != null)
        {
            Player.Instance.transform.position = pairedDoor.transform.position;
            Debug.Log("Player teleported to the paired door.");
        }
        else
        {
            Debug.LogError("Paired door not assigned.");
        }
    }
    
    public bool CanInteract()
    {
        return true;
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        if (!Inventory.Instance.HasItem(requiredItem))
        {
            return new IInteractable.InteractionData("You need a special key to use this portal.");
        }
        return new IInteractable.InteractionData("Use the portal door");
    }
}

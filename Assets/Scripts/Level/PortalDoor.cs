using Characters.Player;
using Interaction;
using UnityEngine;

public class PortalDoor : MonoBehaviour, IInteractable
{
    public PortalDoor pairedDoor; // Reference to the paired door

    // Implement the Interact method from the IInteractable interface
    public void Interact(Player player)
    {
        if (pairedDoor != null)
        {
            // Teleport the player to the paired door's position
            player.transform.position = pairedDoor.transform.position;
            Debug.Log("Player teleported to the paired door.");
        }
        else
        {
            Debug.LogError("Paired door not assigned.");
        }
    }

    // Implement the GetInteractionData method from the IInteractable interface
    public IInteractable.InteractionData GetInteractionData(Player player)
    {
        return new IInteractable.InteractionData("Use the portal door");
    }
}

using Interaction;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour, IInteractable
{
    public GameObject bridgePrefab; // The bridge or rope prefab to spawn
    public Transform anchorPointLeft; // The left side anchor point (this object)
    public Transform anchorPointRight; // The right side anchor point across the chasm
    public string grapplingHookItemName = "GrapplingHook"; // Name of the grappling hook item in the inventory

    private bool bridgeSpawned = false; // To ensure the bridge is spawned only once

    // This is the interaction function, triggered when the player interacts with the anchor point
    public void Interact()
    {
        if (bridgeSpawned)
        {
            Debug.Log("Bridge already spawned.");
            return;
        }

        // Check if the player has the grappling hook in their inventory
        if (Inventory.Instance.HasItem(grapplingHookItemName))
        {
            SpawnBridge();
        }
        else
        {
            Debug.Log("You need a grappling hook to interact with the anchor.");
        }
    }

    // Spawns the bridge prefab between the two anchor points
    private void SpawnBridge()
    {
        if (bridgePrefab != null && anchorPointRight != null)
        {
            // Instantiate the bridge and set its position to be between the two anchor points
            GameObject bridge = Instantiate(bridgePrefab);

            // Adjust the position and scale of the bridge based on the anchor points
            Vector3 midPoint = (anchorPointLeft.position + anchorPointRight.position) / 2;
            bridge.transform.position = midPoint;

            // Calculate the scale or length of the bridge
            float distance = Vector3.Distance(anchorPointLeft.position, anchorPointRight.position);
            bridge.transform.localScale = new Vector3(distance, bridge.transform.localScale.y, bridge.transform.localScale.z);

            bridgeSpawned = true;
            Debug.Log("Bridge spawned between the anchors.");
        }
        else
        {
            Debug.LogError("Bridge prefab or right anchor point not set.");
        }
    }

    public bool CanInteract() => !bridgeSpawned; // Only allow interaction if the bridge hasn't been spawned yet

    // Optionally: Return interaction data if needed by your system (optional)
    public IInteractable.InteractionData? GetInteractionData()
    {
        if (!Inventory.Instance.HasItem(grapplingHookItemName))
        {
            return new IInteractable.InteractionData("You need a grappling hook to spawn the bridge.");
        }
        return new IInteractable.InteractionData("Use the grappling hook to spawn a bridge.");
    }
}

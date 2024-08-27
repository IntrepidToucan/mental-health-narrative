using UnityEngine;

public class WaterDrainTrigger : MonoBehaviour
{
    [SerializeField] private GameObject waterObject; // Reference to the water GameObject
    [SerializeField] private Item requiredItem; // The item that will trigger the water drain

    private void OnEnable()
    {
        FindObjectOfType<ItemPickup>().OnItemPickedUp += HandleItemPickedUp;
    }

    private void OnDisable()
    {
        FindObjectOfType<ItemPickup>().OnItemPickedUp -= HandleItemPickedUp;
    }

    private void HandleItemPickedUp()
    {
        // Check if the player's inventory has the required item
        if (Inventory.Instance.HasItem(requiredItem.itemId))
        {
            if (waterObject != null)
            {
                Destroy(waterObject);
                Debug.Log("Water drained.");
            }
        }
    }
}

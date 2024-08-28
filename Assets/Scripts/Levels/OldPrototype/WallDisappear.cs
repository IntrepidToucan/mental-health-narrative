using UnityEngine;

public class WallDisappearOnItemPickup : MonoBehaviour
{
    public ItemPickup itemPickup; // Reference to the item pickup script
    public GameObject wallToDisappear; // Reference to the wall that should disappear

    private void Start()
    {
        if (itemPickup != null)
        {
            // Subscribe to the item pickup event
            itemPickup.OnItemPickedUp += HandleItemPickedUp;
        }
        else
        {
            Debug.LogError("ItemPickup reference is not assigned.");
        }

        if (wallToDisappear == null)
        {
            Debug.LogError("Wall to disappear reference is not assigned.");
        }
    }

    private void OnDestroy()
    {
        if (itemPickup != null)
        {
            // Unsubscribe from the item pickup event to avoid memory leaks
            itemPickup.OnItemPickedUp -= HandleItemPickedUp;
        }
    }

    private void HandleItemPickedUp()
    {
        if (wallToDisappear != null)
        {
            wallToDisappear.SetActive(false); // Disable the wall
            Debug.Log("Wall has disappeared.");
        }
    }
}

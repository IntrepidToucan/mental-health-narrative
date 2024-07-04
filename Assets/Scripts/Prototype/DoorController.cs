using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string requiredItem;
    public GameObject door; // The door object itself
    public Animator animator; // Optional: Animator for door animations
    public AudioClip unlockSound; // Optional: Sound effect for unlocking

    private void Start()
    {
        // Subscribe to the inventory update event
        Inventory.OnInventoryUpdated += CheckUnlockCondition;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        Inventory.OnInventoryUpdated -= CheckUnlockCondition;
    }

    private void CheckUnlockCondition()
    {
        // Check if the required item is in the inventory
        if (Inventory.Instance.HasItem(requiredItem))
        {
            UnlockDoor();
        }
    }

    private void UnlockDoor()
    {
        // Play unlock sound if set
        if (unlockSound != null)
        {
            AudioSource.PlayClipAtPoint(unlockSound, transform.position);
        }

        // Trigger an animation if an Animator is attached
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        // Optionally deactivate the door GameObject after animations
        // You could also wait for the animation to finish if needed
        Invoke(nameof(DeactivateDoor), 1.0f); // Wait for 1 second before deactivating

        Debug.Log("Door Unlocked!");
    }

    private void DeactivateDoor()
    {
        door.SetActive(false);
    }
}

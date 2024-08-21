using UnityEngine;
using Interaction;

public class ExplosionTrigger : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private RedPlatformMovement redPlatformMovement; // Reference to the red platform movement script
    [SerializeField] private Rigidbody2D yellowPillar;                // Reference to the yellow pillar's Rigidbody2D

    private bool isWirePlaced = false;
    private bool isBoxOnTrigger = false;

    public void Interact()
    {
        if (!isWirePlaced && CheckForRequiredItems())
        {
            isWirePlaced = true;
            RemoveRequiredItems();
            Debug.Log("Wire placed. The device is now armed. Move the box onto the trigger to cause the explosion.");
        }
        else
        {
            Debug.Log("Required items are missing or already placed.");
        }
    }

    private bool CheckForRequiredItems()
    {
        // Assuming some inventory check here
        return true;
    }

    private void RemoveRequiredItems()
    {
        // Assuming items are removed from inventory here
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box") && isWirePlaced)
        {
            isBoxOnTrigger = true;
            TriggerExplosion();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            isBoxOnTrigger = false;
        }
    }

    private void TriggerExplosion()
    {
        if (isWirePlaced && isBoxOnTrigger)
        {
            Debug.Log("Explosion triggered!");

            // Release the red platform
            redPlatformMovement.ReleasePlatform();
        }
    }

    // Implementing the IInteractable interface methods
    public bool CanInteract()
    {
        return !isWirePlaced;  // Interact only if the wire has not been placed yet
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        if (!isWirePlaced)
        {
            return new IInteractable.InteractionData("Press 'E' to place the wire.");
        }
        else
        {
            return new IInteractable.InteractionData("The device is armed. Move the box to trigger the explosion.");
        }
    }
}

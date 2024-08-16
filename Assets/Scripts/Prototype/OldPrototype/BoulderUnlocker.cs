using Interaction;
using UnityEngine;

public class BoulderUnlocker : MonoBehaviour, IInteractable
{
    public string requiredItem; // Item required to unlock the boulder
    public Rigidbody2D boulderRigidbody; // Reference to the boulder's Rigidbody2D
    public bool requiresItem = true; // Can be toggled in the Unity Editor
    public Vector2 forceToApply; // The force to apply to the boulder once unlocked

    private bool isUnlocked = false; // Track if the boulder has been unlocked

    private void Start()
    {
        // Ensure the boulder's Rigidbody2D is set to kinematic initially
        if (boulderRigidbody != null)
        {
            boulderRigidbody.isKinematic = true;
        }
    }

    public void Interact()
    {
        if (isUnlocked)
        {
            Debug.Log("Boulder is already unlocked.");
            return;
        }

        if (requiresItem && !Inventory.Instance.HasItem(requiredItem))
        {
            Debug.Log("You need a special item to unlock this boulder.");
            return;
        }

        // Unlock the boulder
        if (boulderRigidbody != null)
        {
            boulderRigidbody.isKinematic = false;
            boulderRigidbody.AddForce(forceToApply, ForceMode2D.Impulse);
            isUnlocked = true; // Mark the boulder as unlocked
            Debug.Log("Boulder unlocked and released.");
        }
        else
        {
            Debug.LogError("Boulder Rigidbody2D not assigned.");
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        if (requiresItem && !Inventory.Instance.HasItem(requiredItem))
        {
            return new IInteractable.InteractionData("You need a special item to unlock this boulder.");
        }
        return new IInteractable.InteractionData("Unlock the boulder");
    }
}

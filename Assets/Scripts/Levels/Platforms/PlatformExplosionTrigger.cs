using Interaction;
using UnityEngine;

public class PlatformExplosionTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private PlatformExplosion platformExplosion; // Reference to the PlatformExplosion script

    private bool isActivated = false; // To ensure the explosion only happens once

    public void Interact()
    {
        if (!isActivated)
        {
            isActivated = true;
            platformExplosion.TriggerExplosion();
            Debug.Log("Platform explosion triggered!");
        }
    }

    public bool CanInteract()
    {
        return !isActivated; // Allow interaction only if the explosion hasn't been triggered yet
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        return new IInteractable.InteractionData("Press 'E' to trigger the explosion.");
    }
}

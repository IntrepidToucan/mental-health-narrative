using Interaction;
using UnityEngine;

public class ShovelInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject dirtPile;  // Reference to the dirt pile
    [SerializeField] private GameObject hiddenFlower;  // The flower hidden under the dirt pile

    private bool isUsed = false;

    public bool CanInteract()
    {
        return !isUsed;  // Can only interact if the shovel hasn't been used yet
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        return new IInteractable.InteractionData("Press 'E' to use the shovel");
    }

    public void Interact()
    {
        if (!isUsed)
        {
            isUsed = true;
            RevealFlower();
        }
    }

    private void RevealFlower()
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
    }
}

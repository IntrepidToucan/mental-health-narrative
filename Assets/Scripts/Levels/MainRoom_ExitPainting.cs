using UnityEngine;
using Interaction;
using System.Collections;
using System.Collections.Generic;


public class MainRoom_ExitPainting : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject initialPainting;  // The original painting the player entered through
    [SerializeField] private GameObject transformedPainting;  // The new painting to appear after the player exits

    private bool isObjectiveComplete = false;

    // This allows the player to interact with the exit painting if the objective isn't complete
    public bool CanInteract()
    {
        return !isObjectiveComplete;
    }

    // Shows an interaction prompt when the player is near the exit painting
    public IInteractable.InteractionData? GetInteractionData()
    {
        return new IInteractable.InteractionData("Press 'E' to exit through the painting");
    }

    // This is called when the player interacts with the exit painting
    public void Interact()
    {
        if (!isObjectiveComplete)
        {
            isObjectiveComplete = true;
            TransformPainting();
        }
    }

    // This method changes the original painting to the new transformed painting
    private void TransformPainting()
    {
        if (initialPainting != null) initialPainting.SetActive(false);  // Hide the original painting
        if (transformedPainting != null) transformedPainting.SetActive(true);  // Show the transformed painting
    }
}
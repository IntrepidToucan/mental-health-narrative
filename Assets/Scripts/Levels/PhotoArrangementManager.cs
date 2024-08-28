using UnityEngine;

public class PhotoArrangementManager : MonoBehaviour
{
    public PhotoInteractable[] photos; // Array to hold the photo interactables
    public GameObject pillar; // The pillar that will be broken when the puzzle is solved

    private void Update()
    {
        if (CheckAllPhotosCorrectlyPlaced())
        {
            TriggerPillarDestruction();
        }
    }

    private bool CheckAllPhotosCorrectlyPlaced()
    {
        foreach (PhotoInteractable photo in photos)
        {
            if (!photo.isPlacedCorrectly)
            {
                return false;
            }
        }
        return true;
    }

    private void TriggerPillarDestruction()
    {
        Debug.Log("All photos are in the correct order! Pillar is breaking down.");
        Destroy(pillar); // Replace with an animation or effect if needed
    }
}

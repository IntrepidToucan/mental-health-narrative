using UnityEngine;

public class PhotoInteractable : MonoBehaviour
{
    public int correctPosition; // The correct position index for this photo
    public int currentPosition; // The current position index for this photo
    public bool isPlacedCorrectly; // Check if the photo is in the correct position

    private void Start()
    {
        isPlacedCorrectly = false;
    }

    public void PlacePhoto(int newPosition)
    {
        currentPosition = newPosition;
        CheckPlacement();
    }

    private void CheckPlacement()
    {
        if (currentPosition == correctPosition)
        {
            isPlacedCorrectly = true;
        }
        else
        {
            isPlacedCorrectly = false;
        }
    }
}

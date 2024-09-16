using Interaction;
using UnityEngine;

public class PostItInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject postIt;         // The Post-It note to remove
    [SerializeField] private GameObject platformToSpawn; // The platform that will spawn across the gap
    [SerializeField] private Transform spawnPosition;   // The position where the platform will spawn
    private bool isUsed = false;                        // To ensure the interaction only happens once

    public bool CanInteract()
    {
        return !isUsed; // Can only interact if the interaction hasn't been used yet
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        return new IInteractable.InteractionData("Press 'E' to remove the post-it");
    }

    public void Interact()
    {
        if (!isUsed)
        {
            isUsed = true;
            RemovePostIt();
            SpawnPlatform();
        }
    }

    private void RemovePostIt()
    {
        if (postIt != null)
        {
            Destroy(postIt); // Remove the post-it note from the scene
        }
    }

    private void SpawnPlatform()
    {
        if (platformToSpawn != null && spawnPosition != null)
        {
            Instantiate(platformToSpawn, spawnPosition.position, Quaternion.identity);
            Debug.Log("Platform has been spawned across the gap.");
        }
    }
}

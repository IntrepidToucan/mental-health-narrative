using Characters.Player;
using Interaction;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour, IInteractable
{
    public string requiredItem; // Item required to spawn the platform
    public GameObject platformPrefab; // The platform prefab to spawn
    public Transform spawnLocation; // The location to spawn the platform
    public bool requiresItem = true; // Can be toggled in the Unity Editor

    public void Interact(Player player)
    {
        if (requiresItem && !Inventory.Instance.HasItem(requiredItem))
        {
            Debug.Log("You need a special key to spawn this platform.");
            return;
        }

        // Spawn the platform at the specified location
        if (platformPrefab != null && spawnLocation != null)
        {
            Instantiate(platformPrefab, spawnLocation.position, Quaternion.identity);
            Debug.Log("Platform spawned successfully.");
        }
        else
        {
            Debug.LogError("Platform prefab or spawn location not assigned.");
        }
    }

    public IInteractable.InteractionData GetInteractionData(Player player)
    {
        if (requiresItem && !Inventory.Instance.HasItem(requiredItem))
        {
            return new IInteractable.InteractionData("You need a special key to spawn this platform.");
        }
        return new IInteractable.InteractionData("Spawn the platform");
    }
}

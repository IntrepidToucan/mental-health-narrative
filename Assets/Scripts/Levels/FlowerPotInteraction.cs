using Characters.Player;
using Interaction;
using UnityEngine;

public class FlowerPotInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private int totalFlowerCount = 3;  // Total number of flowers needed to place in the pot
    [SerializeField] private GameObject exitPainting;  // The painting that spawns after placing all flowers
    [SerializeField] private string requiredFlowerItemId;  // The string ID of the flower item required
    [SerializeField] private Transform[] flowerPositions;  // Positions where placed flowers appear in the pot
    [SerializeField] private GameObject flowerPrefab;  // Prefab for the visual flower that appears in the pot

    private int flowersPlaced = 0;
    private Inventory playerInventory;

    private void Start()
    {
        // Ensure the exit painting is initially hidden
        exitPainting.SetActive(false);

        // Find the player's inventory
        playerInventory = Player.Instance.GetComponent<Inventory>();
    }

    public bool CanInteract()
    {
        // The player can interact if there are flowers left to place and they have a flower in their inventory
        return flowersPlaced < totalFlowerCount && playerInventory.HasItem(requiredFlowerItemId);
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        if (playerInventory.HasItem(requiredFlowerItemId))
        {
            return new IInteractable.InteractionData("Press 'E' to place the flower in the pot");
        }
        else
        {
            return new IInteractable.InteractionData("You need a flower to place in the pot");
        }
    }

    public void Interact()
    {
        if (CanInteract())
        {
            PlaceFlowerInPot();
        }
    }

    private void PlaceFlowerInPot()
    {
        // Remove the flower from the player's inventory using the string ID
        playerInventory.RemoveItem(requiredFlowerItemId);

        // Instantiate a flower at the next available position
        if (flowersPlaced < flowerPositions.Length && flowerPrefab != null)
        {
            Instantiate(flowerPrefab, flowerPositions[flowersPlaced].position, Quaternion.identity);
        }

        flowersPlaced++;

        // Check if all flowers have been placed
        if (flowersPlaced >= totalFlowerCount)
        {
            SpawnExitPainting();
        }
    }

    private void SpawnExitPainting()
    {
        // Reveal or instantiate the exit painting
        exitPainting.SetActive(true);
        Debug.Log("All flowers placed! Exit painting has appeared.");
    }
}

using Characters.Player;
using Interaction;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlowerPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject aliveLeaf;  // Reference to the green (alive) leaf platform
    [SerializeField] private Sprite deadLeafSprite;  // Sprite to show when the leaf wilts
    [SerializeField] private Vector3 targetPosition;  // Desired target position to move to
    [SerializeField] private Vector3 targetRotation;  // Desired target rotation (Euler angles) for final orientation
    [SerializeField] private float transformationDuration = 2f;  // Time it takes for the leaf to fully transform (wilt)

    // Item reference for the flower (make sure to assign this in the Inspector)
    [SerializeField] private Item flowerItem;

    private bool isCollected = false;

    // Reference to the player's inventory system
    private Inventory playerInventory;

    private void Start()
    {
        playerInventory = Player.Instance.GetComponent<Inventory>();
    }

    public bool CanInteract()
    {
        return !isCollected;  // Only allow interaction if the flower hasn't been picked up yet
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        return new IInteractable.InteractionData("Press 'E' to pick up the flower");
    }

    public void Interact()
    {
        if (!isCollected)
        {
            isCollected = true;
            CollectFlower();
            WiltLeaf();  // Directly apply the transformation
        }
    }

    private void CollectFlower()
    {
        // Add the flower to the player's inventory
        if (playerInventory != null && flowerItem != null)
        {
            playerInventory.AddItem(flowerItem);
        }

        // Disable or destroy the flower object after collecting
        Destroy(gameObject);
    }
    private void WiltLeaf()
    {
        // Change the sprite of the leaf to the dead leaf sprite
        SpriteRenderer leafRenderer = aliveLeaf.GetComponent<SpriteRenderer>();
        if (leafRenderer != null)
        {
            leafRenderer.sprite = deadLeafSprite;
            Debug.Log("Leaf sprite changed to dead sprite.");
        }

        // Log initial local position and rotation
        Debug.Log($"Initial Local Position: {aliveLeaf.transform.localPosition}, Initial Local Rotation: {aliveLeaf.transform.localRotation.eulerAngles}");

        // Set the target local position and rotation
        aliveLeaf.transform.localPosition = targetPosition;
        aliveLeaf.transform.localRotation = Quaternion.Euler(targetRotation);

        // Log final local position and rotation
        Debug.Log($"New Local Position: {aliveLeaf.transform.localPosition}, New Local Rotation: {aliveLeaf.transform.localRotation.eulerAngles}");
    }
}


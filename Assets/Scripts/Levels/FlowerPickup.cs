using Characters.Player;
using Interaction;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FlowerPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject aliveLeaf;  // Reference to the green (alive) leaf platform
    [SerializeField] private Sprite deadLeafSprite;  // Sprite to show when the leaf wilts
    [SerializeField] private Vector3 targetRotation = new Vector3(0, 0, 45);  // Final rotation (e.g., tilt to form a ramp)
    [SerializeField] private float transformationDuration = 2f;  // Time it takes for the leaf to fully transform (wilt)

    // Item reference instead of string for the flower
    [SerializeField] private Item flowerItem;  // Reference to the flower Item (adjust according to your project)

    private bool isCollected = false;

    // Reference to the player's inventory system
    private Inventory playerInventory;

    private void Start()
    {
        // Find the player's inventory (adapt this to match your project)
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
        }
    }

    private void CollectFlower()
    {
        // Add the flower to the player's inventory using the Item reference
        if (playerInventory != null)
        {
            playerInventory.AddItem(flowerItem);  // Add the Item object to the inventory
        }
        else
        {
            Debug.LogWarning("Player inventory not found.");
        }

        // Trigger the leaf transformation (wilt) process
        if (aliveLeaf != null)
        {
            StartCoroutine(WiltLeaf());
        }

        // Disable or destroy the flower object after collecting
        Destroy(gameObject);
    }

    private IEnumerator WiltLeaf()
    {
        // Change the sprite of the leaf to the dead leaf sprite
        SpriteRenderer leafRenderer = aliveLeaf.GetComponent<SpriteRenderer>();
        if (leafRenderer != null)
        {
            leafRenderer.sprite = deadLeafSprite;
        }

        // Gradually rotate or move the leaf to form a ramp or other shape
        Quaternion initialRotation = aliveLeaf.transform.rotation;
        Quaternion finalRotation = Quaternion.Euler(targetRotation);
        float elapsedTime = 0f;

        while (elapsedTime < transformationDuration)
        {
            aliveLeaf.transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / transformationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        aliveLeaf.transform.rotation = finalRotation;  // Ensure the final rotation is exact
    }
}

using UnityEngine;

public class LeafTransformation : MonoBehaviour
{
    [SerializeField] private GameObject greenLeaf; // Reference to the green leaf platform
    [SerializeField] private GameObject brownLeaf; // Reference to the brown leaf platform
    [SerializeField] private Item requiredItem;    // The item that triggers the transformation

    private void OnEnable()
    {
        FindObjectOfType<ItemPickup>().OnItemPickedUp += HandleItemPickedUp;
    }

    private void OnDisable()
    {
        FindObjectOfType<ItemPickup>().OnItemPickedUp -= HandleItemPickedUp;
    }

    private void HandleItemPickedUp()
    {
        // Check if the player's inventory has the required item
        if (Inventory.Instance.HasItem(requiredItem.itemId))
        {
            TransformLeaf();
        }
    }

    private void TransformLeaf()
    {
        if (greenLeaf != null && brownLeaf != null)
        {
            greenLeaf.SetActive(false);   // Disable the green leaf
            brownLeaf.SetActive(true);    // Enable the brown leaf
            Debug.Log("Leaf has withered and transformed.");
        }
    }
}

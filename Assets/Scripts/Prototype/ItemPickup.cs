using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData; // Drag your Scriptable Object here in the inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        // Add the item to the player's inventory
        Inventory.Instance.AddItem(itemData);
        // Optionally play a sound or animation

        // Destroy the pickup object to simulate the item being picked up
        Destroy(gameObject);
    }
}

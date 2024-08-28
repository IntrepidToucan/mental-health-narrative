using Characters.Player;
using Interaction;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("Items")]
    public Item itemA;
    public Item itemB;
    public Item itemC;
    public Item itemD;
    public Item itemE;
    public Item itemF;
    public Item itemG;
    public Item itemH;

    [Header("Wall")]
    public GameObject wall;

    public void Interact()
    {
        Inventory playerInventory = Inventory.Instance;

        if (playerInventory != null)
        {
            if (playerInventory.HasItem(itemA.itemName))
            {
                playerInventory.AddItem(itemB);
                playerInventory.RemoveItem(itemA.itemName);
                RemoveWall();
                Debug.Log("Given itemB to player and removed wall.");
            }
            else if (playerInventory.HasItem(itemC.itemName))
            {
                playerInventory.AddItem(itemD);
                playerInventory.RemoveItem(itemC.itemName);
                Debug.Log("Given itemD to player.");
            }
            else if (playerInventory.HasItem(itemE.itemName))
            {
                playerInventory.AddItem(itemF);
                playerInventory.RemoveItem(itemE.itemName);
                Debug.Log("Given itemF to player.");
            }
            else if (playerInventory.HasItem(itemG.itemName))
            {
                playerInventory.AddItem(itemH);
                playerInventory.RemoveItem(itemG.itemName);
                Debug.Log("Given itemH to player.");
            }
            else
            {
                Debug.Log("Player does not have the required item.");
            }
        }
        else
        {
            Debug.LogError("Player inventory not found");
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public IInteractable.InteractionData? GetInteractionData()
    {
        Inventory playerInventory = Inventory.Instance;

        if (playerInventory != null)
        {
            if (playerInventory.HasItem(itemA.itemName))
            {
                return new IInteractable.InteractionData("Give itemB");
            }
            else if (playerInventory.HasItem(itemC.itemName))
            {
                return new IInteractable.InteractionData("Give itemD");
            }
            else if (playerInventory.HasItem(itemE.itemName))
            {
                return new IInteractable.InteractionData("Give itemF");
            }
            else if (playerInventory.HasItem(itemG.itemName))
            {
                return new IInteractable.InteractionData("Give itemH");
            }
        }
        return new IInteractable.InteractionData("Cannot interact");
    }

    private void RemoveWall()
    {
        if (wall != null)
        {
            Destroy(wall);
            Debug.Log("Wall removed from the scene.");
        }
        else
        {
            Debug.LogError("Wall GameObject not assigned.");
        }
    }
}

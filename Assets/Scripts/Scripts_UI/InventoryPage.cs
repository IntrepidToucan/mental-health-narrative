using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private InventoryDescription itemDescription;
    [SerializeField] private MouseFollower mouseFollower;

    public Sprite image;
    public int quantity;
    public string title, description;

    private List<InventoryItem> listofUIItems = new List<InventoryItem>();

    private void Awake()
    {
        Debug.Log("InventoryPage Awake");
        Hide();
        mouseFollower.Toggle(false);
        itemDescription.ResetDescription();
    }

    public void InitializeInventoryUI(int inventorySize)
    {
        Debug.Log($"Initializing Inventory UI with size: {inventorySize}");

        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listofUIItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    private void HandleShowItemActions(InventoryItem obj)
    {
        Debug.Log($"HandleShowItemActions called on {obj.name}");
        // Implement item actions (e.g., use, equip, drop)
    }

    private void HandleEndDrag(InventoryItem obj)
    {
        Debug.Log($"HandleEndDrag called on {obj.name}");
        mouseFollower.Toggle(false);
    }

    private void HandleSwap(InventoryItem obj)
    {
        Debug.Log($"HandleSwap called on {obj.name}");
        // Implement item swap logic
    }

    private void HandleBeginDrag(InventoryItem obj)
    {
        Debug.Log($"HandleBeginDrag called on {obj.name}");
        mouseFollower.Toggle(true);
        mouseFollower.SetData(image, quantity);
    }

    private void HandleItemSelection(InventoryItem obj)
    {
        Debug.Log($"Item selected: {obj.name}");
        itemDescription.SetDescription(image, title, description);
        obj.Select();
    }

    public void Show()
    {
        Debug.Log("Showing Inventory");
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        if (listofUIItems.Count > 0)
        {
            listofUIItems[0].SetData(image, quantity);
        }
    }

    public void Hide()
    {
        Debug.Log("Hiding Inventory");
        gameObject.SetActive(false);
    }
}

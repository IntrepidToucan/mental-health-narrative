using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private InventoryDescription itemDescription;
    [SerializeField] private MouseFollower mouseFollower;

    public Sprite image, image2;
    public int quantity;
    public string title, description;

    private List<InventoryItem> listofUIItems = new List<InventoryItem>();

    private int currentlyDraggedItemIndex = -1;

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

    private void HandleShowItemActions(InventoryItem inventoryItemUI)
    {
        Debug.Log($"HandleShowItemActions called on {inventoryItemUI.name}");
        // Implement item actions (e.g., use, equip, drop)
    }

    private void HandleEndDrag(InventoryItem inventoryItemUI)
    {

        mouseFollower.Toggle(false);
    }

    private void HandleSwap(InventoryItem inventoryItemUI)
    {
        int index = listofUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
            return;

        }
        listofUIItems[currentlyDraggedItemIndex].SetData(index == 0 ? image : image2, quantity);
        listofUIItems[index].SetData(currentlyDraggedItemIndex == 0 ? image : image2, quantity);
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(InventoryItem inventoryItemUI)
    {
        int index = listofUIItems.IndexOf(inventoryItemUI);
        if (index == -1)
            return;

        currentlyDraggedItemIndex = index;

        mouseFollower.Toggle(true);
        mouseFollower.SetData(index == 0 ? image : image2 , quantity);
    }

    private void HandleItemSelection(InventoryItem inventoryItemUI)
    {
        Debug.Log($"Item selected: {inventoryItemUI.name}");
        itemDescription.SetDescription(image, title, description);
        inventoryItemUI.Select();
    }

    public void Show()
    {
        Debug.Log("Showing Inventory");
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        
        listofUIItems[0].SetData(image, quantity);
        listofUIItems[1].SetData(image, quantity);
    }

    public void Hide()
    {
        Debug.Log("Hiding Inventory");
        gameObject.SetActive(false);
    }
}

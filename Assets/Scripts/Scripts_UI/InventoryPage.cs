using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryPage : MonoBehaviour
{
  
    [SerializeField] private InventoryItem itemPrefab;

    [SerializeField] private RectTransform contentPanel;

    [SerializeField] private InventoryDescription itemDescription;

    [SerializeField] MouseFollower mouseFollower;

    public Sprite image;
    public int quantity;
    public string title, description;

    List<InventoryItem>listofUIItems = new List<InventoryItem>();


    private void Awake()
    {
        Hide();
        mouseFollower.Toggle(false);
        itemDescription.ResetDescription();
    }

    public void InitializeInventoryUI(int inventorysize)
    {

        for(int i = 0; i < inventorysize; i++)
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

    }  

    private void HandleEndDrag(InventoryItem obj)
    {
        mouseFollower.Toggle(false);
    }

    private void HandleSwap (InventoryItem obj)
    {

    }

    private void HandleBeginDrag(InventoryItem obj)
    {

        mouseFollower.Toggle(true);
        mouseFollower.SetData(image, quantity);

    }

    private void HandleItemSelection(InventoryItem obj)
    {

        Debug.Log(obj.name);
        itemDescription.SetDescription(image, title, description);
        listofUIItems[0].Select();

    }

    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();

        listofUIItems[0].SetData(image, quantity);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


}

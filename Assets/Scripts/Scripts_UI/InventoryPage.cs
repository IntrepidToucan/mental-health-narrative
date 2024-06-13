using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private InventoryItem itemPrefab;

    [SerializeField] private RectTransform contentPanel; 

    List<InventoryItem>ListofUIItems = new List<InventoryItem>();



    public void InitializeInventoryUI(int inventorysize)
    {

        for(int i = 0; i < inventorysize; i++)
        {

            InventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            ListofUIItems.Add(uiItem);
        }

    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


}

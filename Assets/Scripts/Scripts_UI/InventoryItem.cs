using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityTxt;
    [SerializeField] private Image borderImage;

    public event Action<InventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;

    private bool empty = true;

    private void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        Debug.Log("Resetting data");
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        Debug.Log("Setting data");
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = quantity + "";
        empty = false;
    }

    public void Select()
    {
        borderImage.enabled = true;
    }

    public void OnBeginDrag()
    {
        if (empty)
            return;

        Debug.Log("Begin Drag");
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        Debug.Log("Drop");
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag()
    {
        Debug.Log("End Drag");
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        Debug.Log($"Pointer Clicked: {pointerData.button}");
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right Mouse Button Click");
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            Debug.Log("Left Mouse Button Click");
            OnItemClicked?.Invoke(this);
        }
    }
}



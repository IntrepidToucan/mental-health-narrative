using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public ItemId itemId = ItemId.None;
    public string itemName;
    public Sprite itemIcon;
    public Sprite itemPortrait;
    public string[] descriptionLines;
}

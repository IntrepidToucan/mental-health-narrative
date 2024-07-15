using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Inventory.ItemId itemId = Inventory.ItemId.None;
    public string itemName;
    public Sprite itemIcon;
    public string description;
}

using Ink.Runtime;
using UnityEngine;

namespace Interaction
{
    public class InkScript
    {
        public Story Story { get; }
        
        public InkScript(TextAsset inkAsset)
        {
            Story = new Story(inkAsset.text);
            
            Story.BindExternalFunction("hasItem", (string itemIdString) =>
                Inventory.Instance.HasItem(Inventory.TryParseItemId(itemIdString)), true);
            
            Story.BindExternalFunction("updateAffinity", (int delta) =>
            {
                Debug.Log($"updateAffinity: {delta}");
            });
            
            Story.BindExternalFunction("updateInventory", (string itemIdString, int delta) =>
            {
                Debug.Log($"updateInventory: {itemIdString} x{delta}");

                var item = ScriptableObject.CreateInstance<Item>();
                item.itemId = Inventory.TryParseItemId(itemIdString);
                
                Inventory.Instance.AddItem(item);
            });
        }
    }
}

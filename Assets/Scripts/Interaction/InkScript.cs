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
            
            Story.BindExternalFunction("displayPlayerThought", (string text) =>
            {
                Debug.Log($"Player thought: {text}");
            }, true);
            
            Story.BindExternalFunction("hasItem", (string itemIdString) =>
                Inventory.Instance.HasItem(Inventory.TryParseItemId(itemIdString)), true);
            
            Story.BindExternalFunction("updateAffinity", (int delta) =>
            {
                Debug.Log($"updateAffinity: {delta}");
            });
            
            Story.BindExternalFunction("updateInventory", (string itemIdString, int delta) =>
            {
                if (Inventory.Instance.ItemMap.TryGetValue(Inventory.TryParseItemId(itemIdString), out var item))
                {
                    Inventory.Instance.AddItem(item);
                }
                else
                {
                    Debug.LogError($"No item data for {itemIdString}");
                }
            });
        }
    }
}

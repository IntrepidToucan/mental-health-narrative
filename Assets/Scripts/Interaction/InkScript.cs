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
            
            Story.BindExternalFunction("hasItem", (string id) =>
                Inventory.Instance.HasItem(id), true);
            
            Story.BindExternalFunction("updateAffinity", (int delta) =>
            {
                Debug.Log($"updateAffinity: {delta}");
            });
            
            Story.BindExternalFunction("updateInventory", (string itemId, int delta) =>
            {
                Debug.Log($"updateInventory: {itemId} {delta}");

                var item = ScriptableObject.CreateInstance<Item>();
                item.itemId = itemId;
                
                Inventory.Instance.AddItem(item);
            });
        }
    }
}

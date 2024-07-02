using Characters.Player;
using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        public struct InteractionData
        {
            public InteractionData(string prompt)
            {
                InteractionPrompt = prompt;
            }
            
            public string InteractionPrompt;
        }
        
        GameObject gameObject { get; }

        InteractionData GetInteractionData(Player player);
        void Interact(Player player);
    }
}

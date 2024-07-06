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
                Prompt = prompt;
            }
            
            public string Prompt;
        }
        
        GameObject gameObject { get; }

        InteractionData GetInteractionData(Player player);
        void Interact(Player player);
    }
}

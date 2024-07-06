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

        bool CanInteract();
        InteractionData? GetInteractionData();
        void Interact();
    }
}

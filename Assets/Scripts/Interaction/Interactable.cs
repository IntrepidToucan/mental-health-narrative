using Characters.Player;
using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        GameObject gameObject { get; } 
        
        void Interact(Player player);
    }
}

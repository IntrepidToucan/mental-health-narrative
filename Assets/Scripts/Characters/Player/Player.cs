using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerStatsController))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerInteractionController))]
    [RequireComponent(typeof(PlayerDialogueController))]
    public class Player : Character
    {
        public PlayerDialogueController DialogueController { get; private set; }
        public PlayerStatsController StatsController { get; private set; }

        private void Awake()
        {
            DialogueController = GetComponent<PlayerDialogueController>();
            StatsController = GetComponent<PlayerStatsController>();
            
            var layerMask = LayerMask.NameToLayer("Player");

            if (gameObject.layer != layerMask)
            {
                Debug.LogError("Layer not set");
                gameObject.layer = layerMask;
            }
      
            GetComponent<MovementController>().ValidateCollisionMask(
                LayerMask.GetMask("Obstacles", "NPCs"));
        }
    }
}

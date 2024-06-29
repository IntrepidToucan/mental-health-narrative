using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(PlayerDialogueController))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerInteractionController))]
    [RequireComponent(typeof(PlayerParallaxController))]
    [RequireComponent(typeof(PlayerScriptController))]
    [RequireComponent(typeof(PlayerStatsController))]
    [RequireComponent(typeof(PlayerUiController))]
    public class Player : Character
    {
        public PlayerDialogueController DialogueController { get; private set; }
        public PlayerInteractionController InteractionController { get; private set; }
        public PlayerInput PlayerInput { get; private set; }
        public PlayerUiController UiController { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
            DialogueController = GetComponent<PlayerDialogueController>();
            InteractionController = GetComponent<PlayerInteractionController>();
            PlayerInput = GetComponent<PlayerInput>();
            UiController = GetComponent<PlayerUiController>();
            
            var layer= LayerMask.NameToLayer("Player");

            if (gameObject.layer != layer)
            {
                Debug.LogError("Layer not set");
                gameObject.layer = layer;
            }

            GetComponent<MovementController>().ValidateCollisionMask(
                LayerMask.GetMask("NPCs", "Obstacles"));
        }
    }
}

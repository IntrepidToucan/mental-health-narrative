using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Characters.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(PlayerDialogueController))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerInteractionController))]
    [RequireComponent(typeof(InventoryController))]
    public class Player : PersistedSingleton<Player>
    {
        public PlayerInput PlayerInput { get; private set; }
        
        public MovementController MovementController { get; private set; }
        public PlayerDialogueController DialogueController { get; private set; }
        public PlayerInteractionController InteractionController { get; private set; }
        public InventoryController InventoryController { get; private set; }  // Added reference

        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();

            gameObject.tag = "Player";
            gameObject.layer = LayerMask.NameToLayer("Player");
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Player");
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            MovementController = GetComponent<MovementController>();
            MovementController.SetCollisionMask(LayerMask.GetMask("Obstacles"));
            
            PlayerInput = GetComponent<PlayerInput>();
            DialogueController = GetComponent<PlayerDialogueController>();
            InteractionController = GetComponent<PlayerInteractionController>();
            InventoryController = GetComponent<InventoryController>();  // Initialize reference
        }
    }
}

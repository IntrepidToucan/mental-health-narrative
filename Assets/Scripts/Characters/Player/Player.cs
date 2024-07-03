using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Characters.Player
{
    [RequireComponent(typeof(PlayerDialogueController))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerInteractionController))]
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
            
            DontDestroyOnLoad(transform.gameObject);
            
            SceneManager.sceneLoaded += (arg0, mode) =>
            {
                if (SceneManager.GetActiveScene().buildIndex == 0) return;
                
                var door = GameObject.Find("Door");

                if (door is not null)
                {
                    transform.position = door.transform.position + Vector3.down * 0.25f;
                }
            };
            
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

            var spriteRenderer = GetComponent<SpriteRenderer>();
            var sortingLayer = SortingLayer.NameToID("Player");

            if (spriteRenderer.sortingLayerID != sortingLayer)
            {
                Debug.LogError("Sorting layer not set");
                spriteRenderer.sortingLayerID = sortingLayer;
            }

            GetComponent<MovementController>().ValidateCollisionMask(
                LayerMask.GetMask("NPCs", "Obstacles"));
        }
    }
}

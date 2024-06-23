using Ink.Runtime;
using Interaction;
using UnityEngine;

namespace Characters.NPCs
{
    public class Npc : Character, IInteractable
    {
        [Header("Ink")]
        [SerializeField, Tooltip("The compiled Ink JSON file")] private TextAsset inkAsset;
        
        private MovementController _movementController;
        
        public void Interact(Player.Player player)
        {
            player.DialogueController.StartDialogue(this, new Story(inkAsset.text));
        }
        
        private void Awake()
        {
            var layerMask = LayerMask.NameToLayer("NPCs");

            if (gameObject.layer != layerMask)
            {
                Debug.LogError("Layer not set");
                gameObject.layer = layerMask;
            }
            
            _movementController = GetComponent<MovementController>();
            _movementController.ValidateCollisionMask(
                LayerMask.GetMask("Obstacles", "Player"));
        }

        private void Update()
        {
            _movementController.Move(0, false);
        }
    }
}

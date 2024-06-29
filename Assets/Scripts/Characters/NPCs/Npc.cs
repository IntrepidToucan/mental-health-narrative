using Ink.Runtime;
using Interaction;
using UnityEngine;

namespace Characters.NPCs
{
    public class Npc : Character, IInteractable
    {
        [Header("Ink")]
        [SerializeField, Tooltip("The compiled Ink JSON file")] private TextAsset inkAsset;
        
        public void Interact(Player.Player player)
        {
            player.DialogueController.StartDialogue(this, new Story(inkAsset.text));
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            var layer = LayerMask.NameToLayer("NPCs");

            if (gameObject.layer != layer)
            {
                Debug.LogError("Layer not set");
                gameObject.layer = layer;
            }
            
            MovementController.ValidateCollisionMask(
                LayerMask.GetMask("Obstacles", "Player"));
        }

        private void Update()
        {
            MovementController.Move(0, false);
        }
    }
}

using Ink.Runtime;
using Interaction;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TextAsset = UnityEngine.TextAsset;

namespace Characters.NPCs
{
    public class Npc : Character, IInteractable
    {
        [Header("Art")]
        [SerializeField] private FontAsset fontAsset;
        
        [Header("Ink")]
        [SerializeField, Tooltip("The compiled Ink JSON file")] private TextAsset inkAsset;

        public FontAsset Font => fontAsset;

        public IInteractable.InteractionData GetInteractionData(Player.Player player)
        {
            return new IInteractable.InteractionData("Talk");
        }

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
            
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var sortingLayer = SortingLayer.NameToID("NonPlayerObjects");

            if (spriteRenderer.sortingLayerID != sortingLayer)
            {
                Debug.LogError("Sorting layer not set");
                spriteRenderer.sortingLayerID = sortingLayer;
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

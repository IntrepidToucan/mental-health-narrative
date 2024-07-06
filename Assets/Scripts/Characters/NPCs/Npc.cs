using Ink.Runtime;
using Interaction;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TextAsset = UnityEngine.TextAsset;

namespace Characters.NPCs
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MovementController))]
    public class Npc : MonoBehaviour, IInteractable
    {
        [Header("Assets")]
        [SerializeField] private FontAsset fontAsset;
        [SerializeField, Tooltip("The compiled Ink JSON file")] private TextAsset inkAsset;

        public FontAsset Font => fontAsset;
        
        public MovementController MovementController { get; private set; }

        public IInteractable.InteractionData GetInteractionData(Player.Player player)
        {
            return new IInteractable.InteractionData("Talk");
        }

        public void Interact(Player.Player player)
        {
            player.DialogueController.StartDialogue(this, new Story(inkAsset.text));
        }
        
        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("NPCs");
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("NonPlayerObjects");
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            MovementController = GetComponent<MovementController>();
            MovementController.ValidateCollisionMask(LayerMask.GetMask("Obstacles", "Player"));
        }

        private void Update()
        {
            MovementController.Move(0, false);
        }
    }
}

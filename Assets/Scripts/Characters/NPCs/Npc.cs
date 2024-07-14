using Interaction;
using UnityEngine;

namespace Characters.NPCs
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MovementController))]
    public class Npc : MonoBehaviour, IInteractable
    {
        [Header("Data")]
        [SerializeField] private NpcData npcDataOriginal;

        public NpcData NpcData { get; private set; }
        public MovementController MovementController { get; private set; }
        
        public bool CanInteract()
        {
            return true;
        }

        public IInteractable.InteractionData? GetInteractionData()
        {
            if (!CanInteract()) return null;
            
            return new IInteractable.InteractionData("Talk");
        }

        public void Interact()
        {
            if (!CanInteract()) return;
            
            Player.Player.Instance.DialogueController.StartDialogue(this, new InkScript(NpcData.InkAsset));
        }
        
        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("NPCs");
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("NonPlayerObjects");
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            MovementController = GetComponent<MovementController>();
            MovementController.SetCollisionMask(LayerMask.GetMask("Obstacles"));

            NpcData = Instantiate(npcDataOriginal);
        }

        private void Update()
        {
            MovementController.Move(0, false);
        }
    }
}

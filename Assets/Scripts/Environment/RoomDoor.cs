using Characters.Player;
using Interaction;
using Managers;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RoomDoor : MonoBehaviour, ISceneConnectable, IInteractable
    {
        [Header("Params")]
        [SerializeField] private string sceneConnectionId;
        [SerializeField] private string targetSceneName;
        
        [Header("Interaction")]
        [SerializeField] private PlayerHistoryController.HistoryTag grantedHistoryTag =
            PlayerHistoryController.HistoryTag.None;

        private BoxCollider2D _collider;
        
        public string GetSceneConnectionId() => sceneConnectionId;
        public bool CanInteract() => _collider.bounds.Contains(Player.Instance.transform.position);
        
        public IInteractable.InteractionData? GetInteractionData()
        {
            if (!CanInteract()) return null;
            
            return new IInteractable.InteractionData("Enter");
        }

        public void Interact()
        {
            if (!CanInteract()) return;

            if (!PlayerHistoryController.IsDefaultHistoryTag(grantedHistoryTag))
            {
                Player.Instance.HistoryController.AddHistory(grantedHistoryTag);
            }
            
            SceneManager.Instance.SwitchToScene(targetSceneName, this);
        }

        private void Awake()
        {
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("NonPlayerObjects");

            if (string.IsNullOrEmpty(sceneConnectionId))
            {
                Debug.LogError("RoomDoor needs a scene connection ID");
            }

            _collider = GetComponent<BoxCollider2D>();
        }
    }
}

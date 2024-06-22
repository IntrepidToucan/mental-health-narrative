using Interaction;
using UnityEngine;
using UnityEngine.UIElements;

namespace Characters.Player
{
    [RequireComponent(typeof(MovementController))]
    public class PlayerInteractionController : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField, Min(-1)] private int debugRaycastDuration = -1;
        
        [Header("Raycast")]
        [SerializeField, Min(1)] private int rayLength = 1;
        
        [Header("UI")]
        [SerializeField] private GameObject interactionPromptPrefab;
        [SerializeField, Min(0)] private int interactionPromptMarginY = 30;
        
        private const float SkinWidth = 0.015f;
        private const int RayCount = 4;

        private Camera _camera;
        private BoxCollider2D _collider;
        private MovementController _movementController;
        private Player _player;
        
        private IInteractable _interactable;
        private GameObject _interactionPrompt;

        public void TryInteract()
        {
            _interactable?.Interact(_player);
        }

        private void Awake()
        {
            _camera = Camera.main;
            _collider = GetComponent<BoxCollider2D>();
            _movementController = GetComponent<MovementController>();
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            var colliderBounds = _collider.bounds;
            colliderBounds.Expand(SkinWidth * -2);
        
            var origin =  _movementController.DirectionX < Mathf.Epsilon ?
                new Vector2(colliderBounds.min.x, colliderBounds.min.y) :
                new Vector2(colliderBounds.max.x, colliderBounds.min.y);

            var rayDirection = Vector2.right * _movementController.DirectionX;
            var adjustedRayLength = rayLength + SkinWidth;
            var raySpacing = colliderBounds.size.y / (RayCount - 1);
            var layerMask = LayerMask.GetMask("Interaction");

            _interactable = null;
        
            // Skip the first and last rays.
            for (var i = 1; i < RayCount - 1; i++)
            {
                var rayStart = origin + Vector2.up * (raySpacing * i);
                var hit = Physics2D.Raycast(rayStart, rayDirection, adjustedRayLength, layerMask);
            
                if (debugRaycastDuration > Mathf.Epsilon)
                {
                    Debug.DrawRay(rayStart, rayDirection * adjustedRayLength, Color.cyan, debugRaycastDuration);
                }

                if (!hit) continue;

                if (hit.transform.gameObject.GetComponent(typeof(IInteractable)) is IInteractable interactable)
                {
                    _interactable = interactable;
                }

                break;
            }

            if (_interactable != null)
            {
                if (_interactionPrompt is null)
                {
                    _interactionPrompt = Instantiate(interactionPromptPrefab);
                }
                
                var uiDoc = _interactionPrompt.GetComponent<UIDocument>();
                var bounds = _interactable.gameObject.GetComponent<BoxCollider2D>().bounds;
                var position = _interactable.gameObject.transform.position;
                var screenPoint = _camera.WorldToScreenPoint(
                    new Vector3(position.x, bounds.max.y, position.z)); 
                
                var uiRoot = uiDoc.rootVisualElement.Q("root");
                uiRoot.style.left = screenPoint.x - uiRoot.resolvedStyle.width / 2f;
                uiRoot.style.top = _camera.pixelHeight - screenPoint.y - uiRoot.resolvedStyle.height - interactionPromptMarginY;
            }
            else
            {
                if (_interactionPrompt is not null)
                {
                    Destroy(_interactionPrompt);
                    _interactionPrompt = null;
                }
            }
        }
    }
}

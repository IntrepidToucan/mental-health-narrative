using Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerInteractionController : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField, Min(-1)] private int debugRaycastDuration = -1;
        
        [Header("Raycast")]
        [SerializeField, Min(0.1f)] private float rayLength = 1.5f;
        
        [Header("UI")]
        [SerializeField] private GameObject interactionPromptPrefab;
        [SerializeField, Min(0)] private float interactionPromptMarginY = 0.4f;
        
        private const float SkinWidth = 0.015f;
        private const int RayCount = 4;

        private Camera _camera;
        private BoxCollider2D _collider;
        private MovementController _movementController;
        private Player _player;
        private PlayerInput _playerInput;
        
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
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            _interactable = null;

            if (_playerInput.currentActionMap.name == "Player")
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
            }

            if (_interactable != null)
            {
                var bounds = _interactable.gameObject.GetComponent<BoxCollider2D>().bounds;
                var position = _interactable.gameObject.transform.position;
                var targetPosition = new Vector3(position.x, bounds.max.y + interactionPromptMarginY, position.z);
                    
                if (_interactionPrompt is null)
                {
                    _interactionPrompt = Instantiate(interactionPromptPrefab, targetPosition, Quaternion.identity);
                }
                else
                {
                    _interactionPrompt.transform.position = targetPosition;
                }
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

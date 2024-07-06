using System;
using System.Linq;
using Interaction;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
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

        private BoxCollider2D _collider;
        
        private IInteractable _interactable;
        private GameObject _interactionPrompt;

        public void TryInteract()
        {
            _interactable?.Interact();
        }

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _interactable = null;
            _interactionPrompt = null;
        }

        private void Update()
        {
            _interactable = null;

            if (Player.Instance.PlayerInput.currentActionMap.enabled)
            {
                var colliderBounds = _collider.bounds;
                colliderBounds.Expand(SkinWidth * -2);
        
                var origin =  Player.Instance.MovementController.DirectionX < Mathf.Epsilon ?
                    new Vector2(colliderBounds.min.x, colliderBounds.min.y) :
                    new Vector2(colliderBounds.max.x, colliderBounds.min.y);

                var rayDirection = Vector2.right * Player.Instance.MovementController.DirectionX;
                var adjustedRayLength = rayLength + SkinWidth;
                var raySpacing = colliderBounds.size.y / (RayCount - 1);
                var layerMask = LayerMask.GetMask("Interaction");
                
                for (var i = 0; i < RayCount; i++)
                {
                    var rayStart = origin + Vector2.up * (raySpacing * i);
                    var hits = Physics2D.RaycastAll(rayStart, rayDirection, adjustedRayLength, layerMask)
                        .Where(hit =>
                        {
                            var interactableComp = hit.transform.gameObject.GetComponent<IInteractable>();

                            return interactableComp != null && interactableComp.CanInteract();
                        }).ToArray();
                    
                    if (debugRaycastDuration > Mathf.Epsilon)
                    {
                        Debug.DrawRay(rayStart, rayDirection * adjustedRayLength, Color.cyan, debugRaycastDuration);
                    }

                    if (hits.Length <= 0) continue;
                    
                    var rayStart3 = (Vector3)rayStart;
                    
                    Array.Sort(hits, (a, b) =>
                    {
                        var sqrDistanceA = (a.transform.position - rayStart3).sqrMagnitude;
                        var sqrDistanceB = (b.transform.position - rayStart3).sqrMagnitude;

                        if (Mathf.Approximately(sqrDistanceA, sqrDistanceB)) return 0;
                        if (sqrDistanceA > sqrDistanceB) return 1;
                        return -1;
                    });
                    
                    if (hits[0].transform.gameObject.GetComponent(typeof(IInteractable)) is IInteractable interactable)
                    {
                        _interactable = interactable;
                    }

                    break;
                }
            }

            if (_interactable != null)
            {
                var interactableCollider = _interactable.gameObject.GetComponent<BoxCollider2D>();
                var interactableTransform = _interactable.gameObject.transform;
                var targetPosition = new Vector3(interactableTransform.position.x,
                    interactableCollider.bounds.max.y + interactionPromptMarginY, interactableTransform.position.z);

                if (_interactionPrompt == null)
                {
                    _interactionPrompt = Instantiate(interactionPromptPrefab,
                        targetPosition, Quaternion.identity, interactableTransform);
                }
                else
                {
                    _interactionPrompt.transform.position = targetPosition;
                }
            }
            else if (_interactionPrompt != null)
            {
                Destroy(_interactionPrompt);
                _interactionPrompt = null;
            }
        }
    }
}

using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractionCollider : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private float sizeMultiplierX = 2f;
        [SerializeField] private float sizeMultiplierY = 1f;
        
        private void Awake()
        {
            var layer = LayerMask.NameToLayer("Interaction");

            if (gameObject.layer != layer)
            {
                Debug.LogError("Layer not set");
                gameObject.layer = layer;
            }

            var ownCollider = GetComponent<BoxCollider2D>();
            var parentCollider = transform.parent.GetComponentInParent<BoxCollider2D>();
            
            if (!ownCollider.isTrigger)
            {
                Debug.LogError("Interaction collider should be a trigger");
                ownCollider.isTrigger = true;
            }

            var colliderSize = ownCollider.size;
            colliderSize.x = parentCollider.size.x * sizeMultiplierX;
            colliderSize.y = parentCollider.size.y * sizeMultiplierY;
            ownCollider.size = colliderSize;
        }
    }
}

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
            gameObject.layer = LayerMask.NameToLayer("Interaction");
            
            var ownCollider = GetComponent<BoxCollider2D>();
            ownCollider.isTrigger = true;
            
            var parentCollider = transform.parent.GetComponent<BoxCollider2D>();
            var colliderSize = ownCollider.size;
            colliderSize.x = parentCollider.size.x * sizeMultiplierX;
            colliderSize.y = parentCollider.size.y * sizeMultiplierY;
            ownCollider.size = colliderSize;
        }
    }
}

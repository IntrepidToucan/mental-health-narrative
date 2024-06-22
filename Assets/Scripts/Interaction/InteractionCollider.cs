using System;
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
            var layerMask = LayerMask.NameToLayer("Interaction");

            if (gameObject.layer != layerMask)
            {
                Debug.LogError("Layer not set");
                gameObject.layer = layerMask;
            }

            var parentCollider = transform.parent.GetComponentInParent<BoxCollider2D>();
            var colliderComp = GetComponent<BoxCollider2D>();
            
            if (!colliderComp.isTrigger)
            {
                Debug.LogError("Interaction collider should be a trigger");
                colliderComp.isTrigger = true;
            }

            var colliderSize = colliderComp.size;
            colliderSize.x = parentCollider.size.x * sizeMultiplierX;
            colliderSize.y = parentCollider.size.y * sizeMultiplierY;
            colliderComp.size = colliderSize;
        }
    }
}

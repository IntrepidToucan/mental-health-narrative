using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Character : MonoBehaviour
    {
        public MovementController MovementController { get; private set; }

        protected virtual void Awake()
        {
            MovementController = GetComponent<MovementController>();
        }
    }
}

using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(BoxCollider2D))]
    // For its collider to be updated each frame,
    // our object needs a RigidBody2D with Body Type set to "Kinematic"
    // (see https://www.youtube.com/watch?v=OBtaLCmJexk&lc=UgxoGlDrPeHGIM_ILah4AaABAg.8p5HmNy3i3q9IsR_gltMlt).
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour
    {
        private struct CollisionInfo
        {
            public bool Above, Below;
            public bool Left, Right;

            public override string ToString()
            {
                return $"Above: {Above}, Below: {Below}, Left: {Left}, Right: {Right}";
            }

            public void Reset()
            {
                Above = Below = false;
                Left = Right = false;
            }
        }
    
        private struct RaycastOrigins
        {
            public Vector2 TopLeft, TopRight;
            public Vector2 BottomLeft, BottomRight;
        }
    
        private const int MinRayCount = 2;
        private const float SkinWidth = 0.015f;
    
        [Header("Debug")]
        [SerializeField, Min(-1)] private int debugRaycastDuration = -1;
    
        [Header("Raycast")]
        [SerializeField] private LayerMask collisionMask;
        [SerializeField, Min(MinRayCount)] private int horizontalRayCount = 4;
        [SerializeField, Min(MinRayCount)] private int verticalRayCount = 4;
        
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float accelerationTimeGrounded = 0.1f;
        [SerializeField] private float accelerationTimeAirborne = 0.2f;
    
        [Header("Jumping")]
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float distanceToJumpApex = 1.8f;
        [SerializeField] private float fallingGravityScale = 2f;
    
        private BoxCollider2D _collider;
        private CollisionInfo _collisionInfo;

        private Vector3 _velocity;
        private float _velocityXSmoothing;

        public float DirectionX { get; private set; } = 1;

        public void ValidateCollisionMask(LayerMask layerMask)
        {
            if (collisionMask == layerMask) return; 
        
            Debug.LogError("Incorrect collision mask");
            collisionMask = layerMask;
        }

        public void Move(float xMovement, bool isJumping)
        {
            if (_collisionInfo.Above || _collisionInfo.Below)
            {
                _velocity.y = 0;
            }

            if (isJumping && _collisionInfo.Below)
            {
                var jumpInitialVelocity =  2 * jumpHeight * moveSpeed / distanceToJumpApex;
                
                _velocity.y = jumpInitialVelocity;
            }
        
            var gravity = -2 * jumpHeight * Mathf.Pow(moveSpeed, 2) / Mathf.Pow(distanceToJumpApex, 2);

            if (_velocity.y <= Mathf.Epsilon)
            {
                gravity *= fallingGravityScale;
            }

            _velocity.x = Mathf.SmoothDamp(_velocity.x, xMovement * moveSpeed, ref _velocityXSmoothing,
                _collisionInfo.Below ? accelerationTimeGrounded : accelerationTimeAirborne);
            _velocity.y += gravity * Time.deltaTime;

            var frameVelocity = _velocity * Time.deltaTime;
            
            var colliderBounds = _collider.bounds;
            colliderBounds.Expand(SkinWidth * -2);
        
            var raycastOrigins = new RaycastOrigins
            {
                BottomLeft = new Vector2(colliderBounds.min.x, colliderBounds.min.y),
                BottomRight = new Vector2(colliderBounds.max.x, colliderBounds.min.y),
                TopLeft = new Vector2(colliderBounds.min.x, colliderBounds.max.y),
                TopRight = new Vector2(colliderBounds.max.x, colliderBounds.max.y)
            };
            
            _collisionInfo.Reset();

            if (!Mathf.Approximately(frameVelocity.x, 0f))
            {
                AdjustForHorizontalCollisions(ref frameVelocity, raycastOrigins, colliderBounds);
            }

            if (!Mathf.Approximately(frameVelocity.y, 0f))
            {
                AdjustForVerticalCollisions(ref frameVelocity, raycastOrigins, colliderBounds);
            }

            transform.Translate(frameVelocity);
        }

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        
            var rigidBody = GetComponent<Rigidbody2D>();

            if (rigidBody.bodyType != RigidbodyType2D.Kinematic)
            {
                Debug.LogError("Rigid body type must be kinematic");
                rigidBody.bodyType = RigidbodyType2D.Kinematic;
            }
        
            if (horizontalRayCount < MinRayCount)
            {
                Debug.LogError($"Invalid horizontal ray count: {horizontalRayCount}");
                horizontalRayCount = MinRayCount;
            }
        
            if (verticalRayCount < MinRayCount)
            {
                Debug.LogError($"Invalid vertical ray count: {verticalRayCount}");
                verticalRayCount = MinRayCount;
            } 
        }

        private void AdjustForHorizontalCollisions(ref Vector3 velocity, RaycastOrigins raycastOrigins, Bounds colliderBounds)
        {
            DirectionX = Mathf.Sign(velocity.x);
            
            var isGoingLeft = DirectionX < Mathf.Epsilon;
            var origin = isGoingLeft ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;

            var rayDirection = Vector2.right * DirectionX;
            var rayLength = Mathf.Abs(velocity.x) + SkinWidth;
            var raySpacing = colliderBounds.size.y / (horizontalRayCount - 1);
        
            for (var i = 0; i < horizontalRayCount; i++)
            {
                var rayStart = origin + Vector2.up * (raySpacing * i);
                var hit = Physics2D.Raycast(rayStart, rayDirection, rayLength, collisionMask);
            
                if (debugRaycastDuration > Mathf.Epsilon)
                {
                    Debug.DrawRay(rayStart, rayDirection * rayLength, Color.cyan, debugRaycastDuration);
                }

                if (!hit) continue;

                velocity.x = (hit.distance - SkinWidth) * DirectionX;
                // Only trace up to the shortest found collision distance
                // (otherwise, later traces that collide with something at a farther distance
                // would break our adjustment logic).
                rayLength = hit.distance;
            
                _collisionInfo.Left = isGoingLeft;
                _collisionInfo.Right = !isGoingLeft;
            }
        }
    
        private void AdjustForVerticalCollisions(ref Vector3 velocity, RaycastOrigins raycastOrigins, Bounds colliderBounds)
        {
            var directionY = Mathf.Sign(velocity.y);
            var isGoingDown = directionY < Mathf.Epsilon;
            var origin = isGoingDown ? raycastOrigins.BottomLeft : raycastOrigins.TopLeft;

            var rayDirection = Vector2.up * directionY;
            var rayLength = Mathf.Abs(velocity.y) + SkinWidth;
            var raySpacing = colliderBounds.size.x / (verticalRayCount - 1);
        
            for (var i = 0; i < verticalRayCount; i++)
            {
                // Add the x-component of velocity because we want to trace
                // from where we will be after we've moved on the x-axis.
                var rayStart = origin + Vector2.right * (raySpacing * i + velocity.x);
                var hit = Physics2D.Raycast(rayStart, rayDirection, rayLength, collisionMask);
            
                if (debugRaycastDuration > Mathf.Epsilon)
                {
                    Debug.DrawRay(rayStart, rayDirection * rayLength, Color.cyan, debugRaycastDuration);
                }

                if (!hit) continue;

                velocity.y = (hit.distance - SkinWidth) * directionY;
                // Only trace up to the shortest found collision distance
                // (otherwise, later traces that collide with something at a farther distance
                // would break our adjustment logic).
                rayLength = hit.distance;
            
                _collisionInfo.Below = isGoingDown;
                _collisionInfo.Above = !isGoingDown;
            }
        }
    }
}

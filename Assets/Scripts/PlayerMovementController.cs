using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
// For our collider to be updated each frame,
// our object needs a RigidBody2D with Body Type set to "Kinematic"
// (see https://www.youtube.com/watch?v=OBtaLCmJexk&lc=UgxoGlDrPeHGIM_ILah4AaABAg.8p5HmNy3i3q9IsR_gltMlt).
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    public struct CollisionInfo
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
    [SerializeField, Min(-1)] private float debugRaycastDuration = -1f;
    
    [Header("Raycast")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField, Min(MinRayCount)] private int horizontalRayCount = 4;
    [SerializeField, Min(MinRayCount)] private int verticalRayCount = 4;
    
    private BoxCollider2D _collider;
    private CollisionInfo _collisionInfo;

    public CollisionInfo Collisions => _collisionInfo;

    public void Move(Vector3 velocity)
    {
        _collisionInfo.Reset();
        
        var colliderBounds = _collider.bounds;
        colliderBounds.Expand(SkinWidth * -2);
        
        var raycastOrigins = new RaycastOrigins
        {
            BottomLeft = new Vector2(colliderBounds.min.x, colliderBounds.min.y),
            BottomRight = new Vector2(colliderBounds.max.x, colliderBounds.min.y),
            TopLeft = new Vector2(colliderBounds.min.x, colliderBounds.max.y),
            TopRight = new Vector2(colliderBounds.max.x, colliderBounds.max.y)
        };

        if (!Mathf.Approximately(velocity.x, 0f))
        {
            AdjustForHorizontalCollisions(ref velocity, raycastOrigins, colliderBounds);
        }

        if (!Mathf.Approximately(velocity.y, 0f))
        {
            AdjustForVerticalCollisions(ref velocity, raycastOrigins, colliderBounds);
        }

        transform.Translate(velocity);
    }

    private void Start()
    {
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
        
        _collider = GetComponent<BoxCollider2D>();
    }
    
    private void AdjustForHorizontalCollisions(ref Vector3 velocity, RaycastOrigins raycastOrigins, Bounds colliderBounds)
    {
        var directionX = Mathf.Sign(velocity.x);
        var isGoingLeft = directionX < Mathf.Epsilon;
        var origin = isGoingLeft ? raycastOrigins.BottomLeft : raycastOrigins.BottomRight;

        var rayDirection = Vector2.right * directionX;
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

            velocity.x = (hit.distance - SkinWidth) * directionX;
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

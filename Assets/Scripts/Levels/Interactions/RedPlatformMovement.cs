using UnityEngine;

public class RedPlatformMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector2 launchVelocity = new Vector2(10f, 0f);  // The velocity to apply to the platform when released
    private Rigidbody2D rb;
    private bool isReleased = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;  // Start as kinematic so it doesn't move until released
    }

    private void Update()
    {
        if (isReleased)
        {
            // Ensure the platform continues moving after release, if needed
        }
    }

    public void ReleasePlatform()
    {
        isReleased = true;
        rb.bodyType = RigidbodyType2D.Dynamic;  // Set to dynamic to apply physics
        rb.velocity = launchVelocity;  // Apply the velocity to shoot the platform forward
    }
}

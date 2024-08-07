using UnityEngine;
using UnityEngine.Events;

public class BreakablePlatform : MonoBehaviour
{
    [Header("General Settings")]
    public bool isJumpBreakable = false;   // Enable if the platform breaks from jumps
    public bool isImpactBreakable = true;  // Enable if the platform breaks from impacts
    public float platDespawn = 2f;         // Time before platform despawns
    public bool shouldDespawn = true;      // Bool to control despawn logic

    [Header("Jump Break Settings")]
    public int maxJumps = 2; // Number of jumps before breaking
    private int jumpCount = 0; // Counter for jumps

    [Header("Impact Break Settings")]
    public string[] breakableTags = { "OrangePlatform" }; // Tags of objects that can break this platform
    public float breakVelocityThreshold = 5f; // Minimum velocity needed to break the platform
    public Vector2 breakDirection = new Vector2(0, -1); // Direction to check for impact

    [Header("Events")]
    public UnityEvent OnBreak; // Unity Event for additional actions when breaking

    private Rigidbody2D rb;
    private Collider2D platformCollider;
    private bool isBroken = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();

        // Start as kinematic if you want manual control
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Freeze Y position to prevent initial falling
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBroken)
        {
            if (isJumpBreakable)
            {
                HandleJumpBreak(collision);
            }

            if (isImpactBreakable)
            {
                HandleImpactBreak(collision);
            }
        }
    }

    private void HandleJumpBreak(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jumpCount++;
            Debug.Log("Jumped " + jumpCount + " times");

            if (jumpCount >= maxJumps)
            {
                BreakPlatform();
            }
        }
    }

    private void HandleImpactBreak(Collision2D collision)
    {
        foreach (var tag in breakableTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (otherRb != null && CanBreak(otherRb.velocity))
                {
                    BreakPlatform();
                }
                break;
            }
        }
    }

    private bool CanBreak(Vector2 velocity)
    {
        float impactVelocity = Vector2.Dot(velocity, breakDirection.normalized);
        Debug.Log($"Impact Velocity: {impactVelocity}, Threshold: {breakVelocityThreshold}");
        return impactVelocity >= breakVelocityThreshold;
    }

    void BreakPlatform()
    {
        Debug.Log("Platform breaking");

        isBroken = true;

        // Trigger break event for additional actions
        OnBreak?.Invoke();

        // Disable the platform's collider
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }

        // Release Y position constraint to allow falling
        rb.constraints = RigidbodyConstraints2D.None;

        // Change the Rigidbody2D to dynamic to simulate falling if not already dynamic
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        // Optionally despawn the platform after a certain time
        if (shouldDespawn)
        {
            Destroy(gameObject, platDespawn);
        }
    }
}

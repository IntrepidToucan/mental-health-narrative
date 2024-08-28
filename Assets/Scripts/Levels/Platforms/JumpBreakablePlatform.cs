using UnityEngine;

public class JumpBreakablePlatform : MonoBehaviour
{
    public int maxJumps = 2; // Number of jumps before breaking
    private int jumpCount = 0; // Counter for jumps
    public float platDespawn = 2f; // Time before platform despawns
    public bool shouldDespawn = true; // Bool to control despawn logic

    private Rigidbody2D rb;
    private Collider2D platformCollider;
    private bool isBroken = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        Debug.Log($"Jump-Breakable Platform Initialized. Collider enabled: {platformCollider.enabled}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBroken && collision.gameObject.CompareTag("Player"))
        {
            jumpCount++;
            if (jumpCount >= maxJumps)
            {
                BreakPlatform();
            }
        }
    }

    void BreakPlatform()
    {
        isBroken = true;
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.constraints = RigidbodyConstraints2D.None;
        }

        if (shouldDespawn)
        {
            Destroy(gameObject, platDespawn);
        }
    }
}

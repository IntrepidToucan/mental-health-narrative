using UnityEngine;

public class ImpactBreakablePlatform : MonoBehaviour
{
    public float platDespawn = 2f; // Time before platform despawns
    public bool shouldDespawn = true; // Bool to control despawn logic

    private Rigidbody2D rb;
    private Collider2D platformCollider;
    private bool isBroken = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        Debug.Log($"Impact-Breakable Platform Initialized. Collider enabled: {platformCollider.enabled}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isBroken && other.CompareTag("Breaker"))
        {
            BreakPlatform();
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
            rb.constraints = RigidbodyConstraints2D.None;
        }

        if (shouldDespawn)
        {
            Destroy(gameObject, platDespawn);
        }
    }
}

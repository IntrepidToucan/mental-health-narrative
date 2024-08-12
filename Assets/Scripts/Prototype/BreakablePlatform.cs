using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private bool isBreakableByPlayer = true; // Toggle for player-breakable platforms
    [SerializeField] private int maxJumpsBeforeBreak = 3;
    [SerializeField] private float breakForce = 10f;
    [SerializeField] private float despawnTime = 2f; // Time before platform despawns after breaking

    private int currentJumps = 0;
    private bool isBroken = false;
    private Rigidbody2D rb;
    private BoxCollider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        rb.isKinematic = true; // Initially, the platform should not be affected by physics
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isBreakableByPlayer)
        {
            RegisterJump();
        }
        else if (collision.gameObject.CompareTag("BreakablePlatform") && isBroken)
        {
            BreakablePlatform otherPlatform = collision.gameObject.GetComponent<BreakablePlatform>();
            if (otherPlatform != null && !otherPlatform.isBreakableByPlayer) // Ensure only non-player-breakable platforms break
            {
                otherPlatform.Break();
            }
        }
    }

    private void RegisterJump()
    {
        if (isBroken) return;

        currentJumps++;

        if (currentJumps >= maxJumpsBeforeBreak)
        {
            Break();
        }
    }

    private void Break()
    {
        isBroken = true;
        rb.isKinematic = false; // Enable physics so the platform can fall
        rb.AddForce(Vector2.down * breakForce, ForceMode2D.Impulse); // Apply an initial force downward
        Invoke(nameof(Despawn), despawnTime); // Start the despawn timer
    }

    private void Despawn()
    {
        Destroy(gameObject); // Despawn (destroy) the platform
    }
}

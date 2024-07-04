using UnityEngine;

public class BreakableEnvironment : MonoBehaviour
{
    private int jumpCount = 0; // Counter for jumps
    public int maxJumps = 2;   // Number of jumps before breaking
    public float platDespawn = 2f;

    // Reference to the platform's Rigidbody2D
    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        // Ensure the Rigidbody2D is set to Kinematic
        if (rb.bodyType != RigidbodyType2D.Kinematic)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody2D of the player
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Check if the player is moving downward
                if (playerRb.velocity.y <= 0)
                {
                    Debug.Log("Player landed on the platform");

                    // Increment jump count
                    jumpCount++;

                    // Trigger break platform logic if jump count exceeds max jumps
                    if (jumpCount >= maxJumps)
                    {
                        BreakPlatform();
                    }
                }
            }
        }
    }

    void BreakPlatform()
    {
        // Set the platform to break or perform any other logic
        Debug.Log("Platform breaking logic");

        // Example: Change Rigidbody to Dynamic and destroy the platform
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, platDespawn); // Destroy platform after 2 seconds (adjust as needed)
    }
}

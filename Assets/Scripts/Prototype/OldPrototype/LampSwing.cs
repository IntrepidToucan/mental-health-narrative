using UnityEngine;

public class GentlePendulumSwing : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] private float swingAngleOffset = 10f;  // Initial angle offset to start the swing
    [SerializeField] private float angularDrag = 0.05f;     // Drag to slow down the swing over time

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularDrag = angularDrag;

        // Set an initial small rotation to start the swing
        transform.rotation = Quaternion.Euler(0, 0, swingAngleOffset);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the player's Rigidbody2D
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null && playerRb.isKinematic)
            {
                // Temporarily set the player to dynamic to minimize impact on the swing
                playerRb.isKinematic = false;

                // Optionally set mass to very low to reduce impact
                playerRb.mass = 0.1f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reset the player's Rigidbody2D to kinematic
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.isKinematic = true;

                // Reset the mass to its original value if changed
                playerRb.mass = 1f; // or whatever the original mass was
            }
        }
    }
}

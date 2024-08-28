using UnityEngine;

public class PillarCollision : MonoBehaviour
{
    [SerializeField] private GameObject blackWall;  // Reference to the black wall
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;  // Keep the pillar static initially
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RedPlatform"))
        {
            Debug.Log("Red platform hit the yellow pillar. It is now falling.");
            rb.bodyType = RigidbodyType2D.Dynamic;  // Set to dynamic to allow the pillar to fall
        }
        else if (collision.gameObject == blackWall)
        {
            Debug.Log("Pillar collided with the black wall!");
            Destroy(blackWall);  // Destroy the black wall on collision
        }
    }
}

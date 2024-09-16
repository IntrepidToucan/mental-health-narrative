using UnityEngine;

public class RockLoosening : MonoBehaviour
{
    public Rigidbody2D rockRigidbody; // The Rigidbody2D component of the rock to apply force
    public int requiredJumps = 3; // Number of jumps required to loosen the rock
    public Vector2 popForce = new Vector2(200f, 300f); // The force applied to the rock when it pops
    private int currentJumps = 0; // Tracks how many times the player has jumped on the branch
    private bool isLoose = false; // Track if the rock has already been loosened

    private void Start()
    {
        // Ensure the rock is kinematic at the start to prevent any unintended movement
        if (rockRigidbody != null)
        {
            rockRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            Debug.LogError("Rigidbody2D not assigned to rock.");
        }
    }

    // This function triggers when something collides with the branch (e.g., player jumping on it)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only respond to collisions with the player and when the rock isn't already loosened
        if (!isLoose && collision.gameObject.CompareTag("Player"))
        {
            currentJumps++;
            Debug.Log("Player jumped on the branch. Jump count: " + currentJumps);

            // Check if the required number of jumps have been made
            if (currentJumps >= requiredJumps)
            {
                LoosenRock();
            }
        }
    }

    private void LoosenRock()
    {
        isLoose = true; // Mark the rock as loosened

        // Ensure the rock is set to dynamic before applying the force
        if (rockRigidbody != null)
        {
            rockRigidbody.bodyType = RigidbodyType2D.Dynamic;
            rockRigidbody.constraints = RigidbodyConstraints2D.None; // Remove any movement constraints if necessary

            // Apply the pop force to the rock to make it "pop" out dynamically
            rockRigidbody.AddForce(popForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody2D not assigned to rock.");
        }
    }
}

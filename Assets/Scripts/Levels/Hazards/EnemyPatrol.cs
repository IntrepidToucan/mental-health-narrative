using UnityEngine;
using Characters.Player;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;   // Array of waypoints for the enemy to patrol between
    [SerializeField] private float speed = 2f;        // Speed at which the enemy moves
    [SerializeField] private int damage = 1;  // Damage dealt to the player on contact

    private int currentWaypointIndex = 0;             // Index of the current waypoint
    private bool movingForward = true;                // Direction of movement along the path
    private bool playerInvisible = false;  // Flag for whether the player is invisible

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (waypoints.Length == 0 || playerInvisible)
            return;  // Stop patrolling if the player is invisible

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (movingForward)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    movingForward = false;
                    currentWaypointIndex = waypoints.Length - 2;
                }
            }
            else
            {
                currentWaypointIndex--;
                if (currentWaypointIndex < 0)
                {
                    movingForward = true;
                    currentWaypointIndex = 1;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerInvisible)  // Only deal damage if the player isn't invisible
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    // This method will be called by the MirrorInteraction script to toggle player visibility
    public void SetPlayerInvisible(bool isInvisible)
    {
        playerInvisible = isInvisible;
    }
}

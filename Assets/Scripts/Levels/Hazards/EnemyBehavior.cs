using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float approachSpeed = 2f;  // Speed when slowly approaching
    [SerializeField] private float chaseSpeed = 5f;     // Speed when chasing the player
    [SerializeField] private float chaseDistance = 5f;  // Distance at which the enemy starts chasing

    private Transform player;
    private bool isChasing = false;
    private bool playerInvisible = false;  // Flag for whether the player is invisible

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartChase()
    {
        if (!playerInvisible)  // Only chase the player if they aren't invisible
        {
            StartCoroutine(ApproachAndChase());
        }
    }

    private IEnumerator ApproachAndChase()
    {
        // Slowly approach the player
        while (Vector2.Distance(transform.position, player.position) > chaseDistance && !playerInvisible)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, approachSpeed * Time.deltaTime);
            yield return null;
        }

        if (!playerInvisible)  // Only start chasing if the player isn't invisible
        {
            isChasing = true;
        }
    }

    private void Update()
    {
        if (isChasing && !playerInvisible)  // Only chase if the player isn't invisible
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
    }

    // This method will be called by the MirrorInteraction script to toggle player visibility
    public void SetPlayerInvisible(bool isInvisible)
    {
        playerInvisible = isInvisible;

        if (playerInvisible)
        {
            isChasing = false;  // Stop chasing if the player becomes invisible
        }
    }
}

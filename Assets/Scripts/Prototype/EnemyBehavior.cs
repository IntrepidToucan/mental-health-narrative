using System.Collections;  
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float approachSpeed = 2f;  // Speed when slowly approaching
    [SerializeField] private float chaseSpeed = 5f;     // Speed when chasing the player
    [SerializeField] private float chaseDistance = 5f;  // Distance at which the enemy starts chasing

    private Transform player;
    private bool isChasing = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartChase()
    {
        StartCoroutine(ApproachAndChase());
    }

    private IEnumerator ApproachAndChase()
    {
        // Slowly approach the player
        while (Vector2.Distance(transform.position, player.position) > chaseDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, approachSpeed * Time.deltaTime);
            yield return null;
        }

        // Start chasing the player
        isChasing = true;
    }

    private void Update()
    {
        if (isChasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
    }
}

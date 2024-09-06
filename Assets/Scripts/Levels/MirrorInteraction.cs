using Characters.Player;
using UnityEngine;

public class MirrorInteraction : MonoBehaviour
{
    [SerializeField] private GameObject mirror;  // Reference to the mirror
    [SerializeField] private GameObject monsterReflection;  // The monster reflection object
    [SerializeField] private GameObject playerReflection;  // The player's reflection object
    [SerializeField] private float detectionRange = 2f;  // Distance from the mirror to trigger reflection change

    private bool playerInRange = false;

    private void Update()
    {
        DetectPlayerInFrontOfMirror();
    }

    private void DetectPlayerInFrontOfMirror()
    {
        // Check if the player is within range of the mirror
        float distanceToPlayer = Vector2.Distance(Player.Instance.transform.position, mirror.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!playerInRange)
            {
                ShowMonsterReflection();
                playerInRange = true;
            }
        }
        else
        {
            if (playerInRange)
            {
                ShowPlayerReflection();
                playerInRange = false;
            }
        }
    }

    private void ShowMonsterReflection()
    {
        // Show the monster reflection and hide the player's reflection
        monsterReflection.SetActive(true);
        playerReflection.SetActive(false);

        // Disable enemies' ability to see or harm the player
        SetEnemiesInvisible(true);
    }

    private void ShowPlayerReflection()
    {
        // Show the player's reflection and hide the monster
        monsterReflection.SetActive(false);
        playerReflection.SetActive(true);

        // Re-enable enemies' ability to see or harm the player
        SetEnemiesInvisible(false);
    }

    // This method will loop through all enemies in the scene and toggle their visibility
    private void SetEnemiesInvisible(bool isInvisible)
    {
        var enemies = FindObjectsOfType<EnemyBehavior>();  // Find all enemies with the EnemyBehavior script
        foreach (var enemy in enemies)
        {
            enemy.SetPlayerInvisible(isInvisible);  // Set player invisibility for each enemy
        }

        var patrolEnemies = FindObjectsOfType<EnemyPatrol>();  // Find all enemies with the EnemyPatrol script
        foreach (var patrolEnemy in patrolEnemies)
        {
            patrolEnemy.SetPlayerInvisible(isInvisible);  // Set player invisibility for each patrol enemy
        }
    }
}

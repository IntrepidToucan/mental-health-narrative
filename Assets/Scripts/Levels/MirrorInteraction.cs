using Characters.Player;
using UnityEngine;

public class MirrorInteraction : MonoBehaviour
{
    [SerializeField] private GameObject monsterReflection;  // The monster that represents the player's reflection
    [SerializeField] private GameObject playerReflection;  // The player's normal reflection (if needed)
    [SerializeField] private float detectionRange = 2f;  // How close the player needs to be to trigger the reflection change
    [SerializeField] private GameObject mirror;  // The mirror object

    private bool playerInRange = false;

    private void Update()
    {
        DetectPlayerInFrontOfMirror();
    }

    private void DetectPlayerInFrontOfMirror()
    {
        // Check the distance between the player and the mirror
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

    // Show the monster reflection instead of the player
    private void ShowMonsterReflection()
    {
        // Hide the player reflection (if there's one)
        if (playerReflection != null)
        {
            playerReflection.SetActive(false);
        }

        // Show the monster reflection
        if (monsterReflection != null)
        {
            monsterReflection.SetActive(true);
        }

        // Disable enemies' ability to detect the player
        SetPlayerInvisibleToEnemies(true);
    }

    // Switch back to showing the player's reflection when the player leaves the mirror's range
    private void ShowPlayerReflection()
    {
        // Show the player's reflection (if needed)
        if (playerReflection != null)
        {
            playerReflection.SetActive(true);
        }

        // Hide the monster reflection
        if (monsterReflection != null)
        {
            monsterReflection.SetActive(false);
        }

        // Re-enable enemies' ability to detect the player
        SetPlayerInvisibleToEnemies(false);
    }

    // Disable/Enable enemies' detection of the player
    private void SetPlayerInvisibleToEnemies(bool isInvisible)
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

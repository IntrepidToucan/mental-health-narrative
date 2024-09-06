using UnityEngine;

namespace Characters.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 1; // In Limbo, the player typically has only 1 HP.
        private int currentHealth;

        private bool isDead = false;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (isDead) return;

            currentHealth -= amount;
            Debug.Log($"Player took {amount} damage, current health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            Debug.Log("Player has died.");

            // Trigger any death animations or effects here
            PlayerDeath();

            // Restart the level or return to the last checkpoint
            RestartLevel();
        }

        private void PlayerDeath()
        {
            // Handle player death animation or effects
            // (e.g., play a death animation, stop player movement, etc.)
        }

        private void RestartLevel()
        {
            // Restart the level after a delay, or handle respawn logic
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            isDead = false;
        }
    }
}

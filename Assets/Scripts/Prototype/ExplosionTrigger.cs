using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private PlatformExplosion platformExplosion; // Reference to the PlatformExplosion script
    [SerializeField] private string triggeringTag = "Box"; // The tag of the object that can trigger the explosion (e.g., "Box")

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the triggering object is the one we want (e.g., a box or other object)
        if (other.CompareTag(triggeringTag))
        {
            platformExplosion.TriggerExplosion();
        }
    }
}

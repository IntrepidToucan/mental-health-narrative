using System.Collections;
using UnityEngine;

public class PillarFall : MonoBehaviour
{
    private Rigidbody2D pillarRigidbody;
    public float fallForce = 10f; // Adjust the force applied to make the pillar fall
    private bool hasFallen = false;
    public float timeBeforeImmovable = 2.0f; // Time to wait before making the pillar immovable again

    private void Start()
    {
        pillarRigidbody = GetComponent<Rigidbody2D>();
        if (pillarRigidbody != null)
        {
            pillarRigidbody.isKinematic = true; // Ensure the pillar is initially static
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the boulder
        if (collision.gameObject.CompareTag("Boulder") && !hasFallen)
        {
            if (pillarRigidbody != null)
            {
                pillarRigidbody.isKinematic = false; // Make the pillar dynamic
                Vector2 forceDirection = new Vector2(1, -1).normalized; // Direction to apply force
                pillarRigidbody.AddForce(forceDirection * fallForce, ForceMode2D.Impulse);
                hasFallen = true;
                StartCoroutine(MakeImmovableAfterDelay());
                Debug.Log("Pillar hit by boulder and is falling over.");
            }
        }
    }

    private IEnumerator MakeImmovableAfterDelay()
    {
        yield return new WaitForSeconds(timeBeforeImmovable);
        if (pillarRigidbody != null)
        {
            pillarRigidbody.velocity = Vector2.zero;
            pillarRigidbody.angularVelocity = 0f;
            pillarRigidbody.isKinematic = true; // Make the pillar immovable again
            Debug.Log("Pillar is now immovable.");
        }
    }
}

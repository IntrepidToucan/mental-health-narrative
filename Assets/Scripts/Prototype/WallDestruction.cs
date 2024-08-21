using UnityEngine;

public class WallDestruction : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("YellowPillar"))
        {
            DestroyWall();
        }
    }

    private void DestroyWall()
    {
        Debug.Log("Black wall destroyed by yellow pillar.");
        // Add any additional effects or animations here
        Destroy(gameObject);  // Destroys the black wall GameObject
    }
}

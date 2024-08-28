using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighImpactBreakablePlatform : MonoBehaviour
{
    public float breakableVelocityThreshold = 10f;
    public float platDespawn = 2f;
    public bool shouldDespawn = true;

    private Rigidbody2D rb;
    private Collider2D platformCollider;
    private bool isBroken = false;

    // Start is called before the first frame update
    void Start()
    {

        // Get Rigidbody2D and Collider2D components

        rb = GetComponent<Rigidbody2D>();
        platformCollider = GetComponent<Collider2D>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check if player has collided with platform 

        if (collision.gameObject.CompareTag("Player"))
        {
            //get player rigidbody component 

            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            //calculate impact velocity 

            float impactVelocity = Mathf.Abs(playerRb.velocity.x);

            //Debug log for checking velocity impact 

            Debug.Log("Impact velocity: " + impactVelocity);

            //if the impact velocity is greater than threshold, break platform

            if (impactVelocity >= breakableVelocityThreshold && !isBroken)
            {
                BreakPlatform();
            }

        }
    }

    void BreakPlatform()
    {

        Debug.Log("Platform high impact break");

        //set platform as broken 

        isBroken = true;

        // Disable the platform's collider
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }

        // Change the Rigidbody2D to dynamic to simulate falling
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        // Optionally despawn the platform after a certain time
        if (shouldDespawn)
        {
            Destroy(gameObject, platDespawn);
        }
    }


}

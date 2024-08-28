using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTilt : MonoBehaviour
{
    public Transform player; // Reference to player transform 
    public float tiltSpeed = 5f; // Speed of tilt
    public float maxTiltAngle = 15f; // Maximum angle of tilt 
    public float boundaryOffset = 0.5f; // Offset from platform's edge for boundaries 

    private Rigidbody2D rb; 
    private float platformWidth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformWidth = GetComponent<Collider2D>().bounds.size.x; //Get width of platform
    }

    // Update is called once per frame
    void Update()
    {
        TiltPlatform();
    }

    void TiltPlatform()
    {
        // Set platform boundaries 

        float leftBoundary = transform.position.x - (platformWidth/2) + boundaryOffset;
        float rightBoundary = transform.position.x + (platformWidth/2) - boundaryOffset;

        // Determine player position relative to platform 

        if (player.position.x < leftBoundary)
        {

            //player on left 
            TiltTowards(-maxTiltAngle);

        }
        else if(player.position.x > rightBoundary)
        {
            //player on right side
            TiltTowards(maxTiltAngle);

        }
        else
        {
            //player in middle, level platform

            TiltTowards(0);
        }


    }

    void TiltTowards(float targetAngle)
    {

        //calculate current angle 

        float currentAngle = rb.rotation; 

        //lerp toward the target angle for smooth motion 

        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, tiltSpeed * Time.deltaTime);

        //apply the new angle to rigidbody2D

        rb.MoveRotation(newAngle);


    }




}

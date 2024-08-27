using UnityEngine;

public class SinkingPlatform : MonoBehaviour
{
    [SerializeField] private float sinkSpeed = 2f;        // Speed at which the platform sinks
    [SerializeField] private float riseSpeed = 1f;        // Speed at which the platform rises
    [SerializeField] private float sinkDistance = 2f;     // How far the platform sinks
    [SerializeField] private float delayBeforeRising = 0.5f; // Delay before the platform starts rising after the player/box leaves

    private Vector3 initialPosition;                      // The platform's initial position
    private int objectsOnPlatform = 0;                    // Number of objects (player or box) on the platform
    private bool returningToPosition = false;
    private float timeSinceObjectLeft = 0f;

    private void Start()
    {
        initialPosition = transform.position;             // Store the initial position
    }

    private void Update()
    {
        if (objectsOnPlatform > 0 && !returningToPosition)
        {
            // Move the platform down while the player or box is on it
            if (transform.position.y > initialPosition.y - sinkDistance)
            {
                transform.position -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
            }
        }
        else if (objectsOnPlatform == 0)
        {
            // Start or continue the countdown to float back up
            timeSinceObjectLeft += Time.deltaTime;

            if (timeSinceObjectLeft >= delayBeforeRising)
            {
                returningToPosition = true;
                // Move the platform back up to its initial position
                transform.position = Vector3.MoveTowards(transform.position, initialPosition, riseSpeed * Time.deltaTime);

                // If the platform has returned to its initial position, stop moving
                if (transform.position == initialPosition)
                {
                    returningToPosition = false;
                    timeSinceObjectLeft = 0f; // Reset the timer
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            objectsOnPlatform++;
            returningToPosition = false;
            timeSinceObjectLeft = 0f; // Reset the timer since the player or box is back on
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            objectsOnPlatform--;
            timeSinceObjectLeft = 0f; // Start the delay timer
        }
    }
}

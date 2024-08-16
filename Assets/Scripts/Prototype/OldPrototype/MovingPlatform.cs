using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool moveVertical = true; // Enable/disable vertical movement
    public bool moveHorizontal = false; // Enable/disable horizontal movement

    public Vector3 verticalOffsetA = new Vector3(0, 5, 0); // Offset from initial position for vertical point A
    public Vector3 verticalOffsetB = new Vector3(0, -5, 0); // Offset from initial position for vertical point B

    public Vector3 horizontalOffsetA = new Vector3(5, 0, 0); // Offset from initial position for horizontal point A
    public Vector3 horizontalOffsetB = new Vector3(-5, 0, 0); // Offset from initial position for horizontal point B

    public float speed = 2f;

    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 targetPosition;

    private void Start()
    {
        if (moveVertical)
        {
            pointA = transform.position + verticalOffsetA;
            pointB = transform.position + verticalOffsetB;
        }
        else if (moveHorizontal)
        {
            pointA = transform.position + horizontalOffsetA;
            pointB = transform.position + horizontalOffsetB;
        }

        targetPosition = pointB; // Start moving towards pointB
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Switch target position when reaching the current target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = targetPosition == pointA ? pointB : pointA;
        }
    }
}

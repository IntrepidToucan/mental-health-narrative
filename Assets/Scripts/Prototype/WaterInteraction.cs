using UnityEngine;
using UnityEngine.InputSystem;

public class WaterInteraction : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private bool _isInWater = false;
    private float _waterSurfaceY;
    private bool _hasStabilized = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _rigidbody = collision.GetComponent<Rigidbody2D>();
            if (_rigidbody != null)
            {
                _isInWater = true;
                _waterSurfaceY = transform.position.y;  // Ensure this is the Y level of the water's surface
                _hasStabilized = false;  // Reset stabilization flag
                Debug.Log("Player entered water.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isInWater = false;
            _hasStabilized = false;  // Reset stabilization flag
            Debug.Log("Player exited water.");
        }
    }

    private void FixedUpdate()
    {
        if (_isInWater && _rigidbody != null)
        {
            HandleWaterPhysics();
        }
    }

    private void HandleWaterPhysics()
    {
        Vector2 currentPosition = _rigidbody.position;

        // Get horizontal input from the new Input System
        Vector2 input = GetPlayerInput();
        float horizontalInput = input.x;

        // Apply horizontal movement based on input
        Vector2 newPosition = new Vector2(currentPosition.x + horizontalInput * Time.fixedDeltaTime * 5f, currentPosition.y);

        // Check if the player is below the surface and needs to rise
        if (currentPosition.y < _waterSurfaceY)
        {
            _hasStabilized = false;  // Player is still rising
            float riseSpeed = 1.0f;  // Adjust this speed as needed
            newPosition.y += riseSpeed * Time.fixedDeltaTime;  // Apply smooth upward movement
        }
        else
        {
            // Stabilize the player at the surface level
            newPosition.y = _waterSurfaceY;

            // Avoid repeated "stabilized" logs
            if (!_hasStabilized)
            {
                _hasStabilized = true;
                Debug.Log("Player stabilized at the surface.");
            }
        }

        // Apply the final position
        _rigidbody.MovePosition(newPosition);
    }

    private Vector2 GetPlayerInput()
    {
        // Assuming you have a PlayerInput component set up for handling input actions
        PlayerInput playerInput = _rigidbody.GetComponent<PlayerInput>();
        return playerInput.actions["Move"].ReadValue<Vector2>();
    }
}

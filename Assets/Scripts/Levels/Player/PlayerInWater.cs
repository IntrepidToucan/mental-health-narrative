using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInWater : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private bool _isInWater = false;
        private Transform _waterTransform;
        private float _submersionDepth;
        private bool _isFloating = false;

        private InputAction _moveAction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            // Assuming the player has a PlayerInput component attached
            var playerInput = GetComponent<PlayerInput>();
            _moveAction = playerInput.actions["Move"];
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Water"))
            {
                _isInWater = true;
                _waterTransform = collision.transform;
                _submersionDepth = _rigidbody.position.y - 1.0f; // Adjust for initial sinking
                _isFloating = false;
                Debug.Log("Player entered water and will start sinking.");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Water"))
            {
                _isInWater = false;
                _waterTransform = null;
                Debug.Log("Player exited water.");
            }
        }

        private void FixedUpdate()
        {
            if (_isInWater && _waterTransform != null)
            {
                HandleWaterPhysics();
            }
        }

        private void HandleWaterPhysics()
        {
            float surfaceLevel = _waterTransform.position.y;
            Vector2 currentPosition = _rigidbody.position;

            // Get horizontal input from the new Input System
            Vector2 input = _moveAction.ReadValue<Vector2>();
            float horizontalInput = input.x;

            // Apply horizontal movement based on input
            Vector2 newPosition = new Vector2(currentPosition.x + horizontalInput * Time.fixedDeltaTime * 5f, currentPosition.y);

            if (!_isFloating)
            {
                // Sinking phase
                if (currentPosition.y > _submersionDepth)
                {
                    newPosition.y = Mathf.MoveTowards(currentPosition.y, _submersionDepth, 1.0f * Time.fixedDeltaTime);
                }
                else
                {
                    _isFloating = true;
                    Debug.Log("Player has reached sinking depth and will start floating.");
                }
            }
            else
            {
                // Floating phase
                if (currentPosition.y < surfaceLevel)
                {
                    float buoyancyForce = CalculateBuoyancy(surfaceLevel);
                    newPosition.y = Mathf.MoveTowards(currentPosition.y, surfaceLevel, buoyancyForce * Time.fixedDeltaTime);
                }
                else
                {
                    // Stabilize at surface level
                    newPosition.y = surfaceLevel;
                    Debug.Log("Player stabilized at the surface.");
                }
            }

            // Apply the final position
            _rigidbody.MovePosition(newPosition);
        }

        private float CalculateBuoyancy(float surfaceLevel)
        {
            float depthInWater = surfaceLevel - _rigidbody.position.y;
            float buoyancyMultiplier = 2.0f; // Adjust as needed

            return depthInWater * buoyancyMultiplier;
        }
    }
}

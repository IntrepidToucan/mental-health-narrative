using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float accelerationTimeGrounded = 0.1f;
    [SerializeField] private float accelerationTimeAirborne = 0.2f;
    
    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float timeToJumpApex = 0.4f;
    
    private PlayerMovementController _movementController;
    
    private InputAction _moveAction;
    private InputAction _jumpAction;
    
    private Vector3 _velocity;
    private float _velocityXSmoothing;
    
    private void Start()
    {
        _movementController = GetComponent<PlayerMovementController>();
        
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Update()
    {
        if (_movementController.Collisions.Above || _movementController.Collisions.Below)
        {
            _velocity.y = 0;
        }

        var jumpInitialVelocity = 2 * jumpHeight / timeToJumpApex;

        if (_jumpAction.IsPressed() && _movementController.Collisions.Below)
        {
            _velocity.y = jumpInitialVelocity;
        }
        
        var moveValue = _moveAction.ReadValue<Vector2>();
        var gravity = -2 * jumpHeight / Mathf.Pow(timeToJumpApex, 2);

        _velocity.x = Mathf.SmoothDamp(_velocity.x, moveValue.x * moveSpeed, ref _velocityXSmoothing,
            _movementController.Collisions.Below ? accelerationTimeGrounded : accelerationTimeAirborne);
        _velocity.y += gravity * Time.deltaTime;
        _movementController.Move(_velocity * Time.deltaTime);
    }
}
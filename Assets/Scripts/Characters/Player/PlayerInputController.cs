using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(PlayerInteractionController))]
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private MovementController _movementController;
        private PlayerInteractionController _interactionController;
    
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _interactAction;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _movementController = GetComponent<MovementController>();
            _interactionController = GetComponent<PlayerInteractionController>();
        }

        private void Start()
        {
            _moveAction = _playerInput.actions.FindAction("Move");
            _jumpAction = _playerInput.actions.FindAction("Jump");
            _interactAction = _playerInput.actions.FindAction("Interact");

            _interactAction.performed += context =>
            {
                _interactionController.TryInteract();
            };
        }

        private void Update()
        {
            var moveValue = _moveAction.ReadValue<Vector2>();
            
            _movementController.Move(moveValue.x, _jumpAction.IsPressed());
        }
    }
}

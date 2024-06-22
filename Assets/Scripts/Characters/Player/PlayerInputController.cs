using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(PlayerInteractionController))]
    public class PlayerInputController : MonoBehaviour
    {
        private MovementController _movementController;
        private PlayerInteractionController _interactionController;
    
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _interactAction;

        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            _interactionController = GetComponent<PlayerInteractionController>();
        }

        private void Start()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _interactAction = InputSystem.actions.FindAction("Interact");

            _interactAction.started += context =>
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

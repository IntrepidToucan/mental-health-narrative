using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerInputController : MonoBehaviour
    {
        private Player _player;
        
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _interactAction;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            _moveAction = _player.PlayerInput.actions.FindAction("Move");
            _jumpAction = _player.PlayerInput.actions.FindAction("Jump");
            _interactAction = _player.PlayerInput.actions.FindAction("Interact");

            _interactAction.performed += context =>
            {
                _player.InteractionController.TryInteract();
            };
        }

        private void Update()
        {
            var moveValue = _moveAction.ReadValue<Vector2>();
            
            _player.MovementController.Move(moveValue.x, _jumpAction.IsPressed());
        }
    }
}

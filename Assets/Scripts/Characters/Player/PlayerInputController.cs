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
        private InputAction _openLogBookAction;
        private InputAction _pauseGameAction;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            _moveAction = _player.PlayerInput.actions.FindAction("Move");
            _jumpAction = _player.PlayerInput.actions.FindAction("Jump");
            _interactAction = _player.PlayerInput.actions.FindAction("Interact");
            _openLogBookAction = _player.PlayerInput.actions.FindAction("OpenLogBook");
            _pauseGameAction = _player.PlayerInput.actions.FindAction("PauseGame");

            _interactAction.performed += context =>
            {
                _player.InteractionController.TryInteract();
            };
            
            _openLogBookAction.performed += context =>
            {
                Debug.Log("open log book");
            };
            
            _pauseGameAction.performed += context =>
            {
                Debug.Log("pause game");
            };
        }

        private void Update()
        {
            var moveValue = _moveAction.ReadValue<Vector2>();
            
            _player.MovementController.Move(moveValue.x, _jumpAction.IsPressed());
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _interactAction;
        private InputAction _openLogBookAction;
        private InputAction _pauseGameAction;
        private InputAction _inventoryAction;

        private void Start()
        {
            _moveAction = Player.Instance.PlayerInput.actions.FindAction("Move");
            _jumpAction = Player.Instance.PlayerInput.actions.FindAction("Jump");
            _interactAction = Player.Instance.PlayerInput.actions.FindAction("Interact");
            _openLogBookAction = Player.Instance.PlayerInput.actions.FindAction("OpenLogBook");
            _pauseGameAction = Player.Instance.PlayerInput.actions.FindAction("PauseGame");
            _inventoryAction = Player.Instance.PlayerInput.actions.FindAction("Inventory"); // Added for inventory

            _interactAction.performed += context =>
            {
                Player.Instance.InteractionController.TryInteract();
            };

            _openLogBookAction.performed += context =>
            {
                Debug.Log("open log book");
            };

            _pauseGameAction.performed += context =>
            {
                Debug.Log("pause game");
            };

            _inventoryAction.performed += Player.Instance.InventoryController.ToggleInventory;  // Added for inventory
        }

        private void Update()
        {
            Player.Instance.MovementController.Move(
                _moveAction.ReadValue<Vector2>().x, _jumpAction.IsPressed());
        }
    }
}

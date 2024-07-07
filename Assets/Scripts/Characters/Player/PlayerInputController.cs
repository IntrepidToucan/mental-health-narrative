using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private InputAction _interactAction;
        private InputAction _inventoryAction;
        private InputAction _jumpAction;
        private InputAction _moveAction;
        private InputAction _openLogBookAction;
        private InputAction _pauseGameAction;

        private void Start()
        {
            _interactAction = Player.Instance.PlayerInput.actions.FindAction("Interact");
            _inventoryAction = Player.Instance.PlayerInput.actions.FindAction("Inventory");
            _jumpAction = Player.Instance.PlayerInput.actions.FindAction("Jump");
            _moveAction = Player.Instance.PlayerInput.actions.FindAction("Move");
            _openLogBookAction = Player.Instance.PlayerInput.actions.FindAction("OpenLogBook");
            _pauseGameAction = Player.Instance.PlayerInput.actions.FindAction("PauseGame");

            _interactAction.performed += OnInteractPerformed;
            _inventoryAction.performed += OnInventoryPerformed;
            _openLogBookAction.performed += OnOpenLogBookPerformed;
            _pauseGameAction.performed += OnPauseGamePerformed;
        }

        private void Update()
        {
            Player.Instance.MovementController.Move(
                _moveAction.ReadValue<Vector2>().x, _jumpAction.IsPressed());
        }

        private static void OnInteractPerformed(InputAction.CallbackContext context) =>
            Player.Instance.InteractionController.TryInteract();
        private static void OnInventoryPerformed(InputAction.CallbackContext context) =>
            Player.Instance.InventoryController.ToggleInventory(context);
        private static void OnOpenLogBookPerformed(InputAction.CallbackContext context) =>
            Debug.Log("Open log book");
        private static void OnPauseGamePerformed(InputAction.CallbackContext context) =>
            Debug.Log("Pause menu");
    }
}

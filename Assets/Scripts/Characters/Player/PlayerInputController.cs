using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

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
        private InputAction _selectLogBookTabAction;
        private InputAction _selectSettingsTabAction;
        private InputAction _unpauseGameAction;

        private void Start()
        {
            _interactAction = Player.Instance.PlayerInput.actions.FindAction("Interact");
            _inventoryAction = Player.Instance.PlayerInput.actions.FindAction("Inventory");
            _jumpAction = Player.Instance.PlayerInput.actions.FindAction("Jump");
            _moveAction = Player.Instance.PlayerInput.actions.FindAction("Move");
            _openLogBookAction = Player.Instance.PlayerInput.actions.FindAction("OpenLogBook");
            _pauseGameAction = Player.Instance.PlayerInput.actions.FindAction("PauseGame");
            _selectLogBookTabAction = Player.Instance.PlayerInput.actions.FindAction("SelectLogBookTab");
            _selectSettingsTabAction = Player.Instance.PlayerInput.actions.FindAction("SelectSettingsTab");
            _unpauseGameAction = Player.Instance.PlayerInput.actions.FindAction("UnpauseGame");

            _interactAction.performed += OnInteractPerformed;
            _inventoryAction.performed += OnInventoryPerformed;
            _openLogBookAction.performed += OnOpenLogBookPerformed;
            _pauseGameAction.performed += OnPauseGamePerformed;
            _selectLogBookTabAction.performed += OnSelectLogBookTabPerformed;
            _selectSettingsTabAction.performed += OnSelectSettingsTabPerformed;
            _unpauseGameAction.performed += OnUnpauseGamePerformed;
            
            // HACK: Start with a secondary action map enabled
            // and then quickly switch to the primary action map
            // (otherwise, all action maps are enabled from the start--seems to be a Unity bug).
            StartCoroutine(SwitchToPlayerActionMap());
        }
        
        private void Update()
        {
            Player.Instance.MovementController.Move(
                _moveAction.ReadValue<Vector2>().x, _jumpAction.IsPressed());
        }
        
        private static IEnumerator SwitchToPlayerActionMap()
        {
            yield return new WaitForSeconds(0.1f);
            
            Player.Instance.PlayerInput.SwitchCurrentActionMap("Player");
        }

        private static void OnInteractPerformed(InputAction.CallbackContext context) =>
            Player.Instance.InteractionController.TryInteract();
        private static void OnInventoryPerformed(InputAction.CallbackContext context) =>
            Player.Instance.InventoryController.ToggleInventory(context);
        private static void OnOpenLogBookPerformed(InputAction.CallbackContext context) =>
            UiManager.Instance.OpenPauseMenu(PauseMenuTab.LogBook);
        private static void OnPauseGamePerformed(InputAction.CallbackContext context) =>
            UiManager.Instance.OpenPauseMenu();
        private static void OnSelectLogBookTabPerformed(InputAction.CallbackContext context) =>
            UiManager.Instance.OpenPauseMenu(PauseMenuTab.LogBook);
        private static void OnSelectSettingsTabPerformed(InputAction.CallbackContext context) =>
            UiManager.Instance.OpenPauseMenu();
        private static void OnUnpauseGamePerformed(InputAction.CallbackContext context) =>
            UiManager.Instance.ClosePauseMenu();
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Player

{
    public class Player : MonoBehaviour, InputActions.IPlayerActions
    {
        private InputActions _inputActions;
        
        public void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputActions();
                // Tell the "gameplay" action map that we want to get told about
                // when actions get triggered.
                _inputActions.Player.SetCallbacks(this);
            }
            _inputActions.Player.Enable();
        }

        public void OnDisable()
        {
            _inputActions.Player.Disable();
        }

        private void Update()
        {
            var moveVector = _inputActions.Player.Move.ReadValue<Vector2>();
            
            transform.Translate(moveVector.x * 10 * Time.deltaTime, moveVector.y
                * 10 * Time.deltaTime, 0);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
        }

        void InputActions.IPlayerActions.OnJump(InputAction.CallbackContext context)
        {
            Debug.Log("Jump!");
        }
    }
}

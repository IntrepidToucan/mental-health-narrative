using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Player

{
    public class Player : MonoBehaviour, InputActions.IPlayerActions
    {
        private InputActions _inputActions;
        private Rigidbody2D _rigidbody;
        public float moveSpeed = 10.0f;
        public float jumpForce = 1.0f;
        private bool _isGrounded;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
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







        public void OnMove(InputAction.CallbackContext context)
        {

            Vector2 moveInput = context.ReadValue<Vector2>();
            Vector2 moveVector = new Vector2(moveInput.x*moveSpeed,_rigidbody.velocity.y);
            _rigidbody.velocity = moveVector;

        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed && _isGrounded)
            {
                _rigidbody.AddForce(Vector2.up *jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump!");
            }
           
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true; 
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Ground"))
            {
                _isGrounded = false;
            }
        }


    }
}

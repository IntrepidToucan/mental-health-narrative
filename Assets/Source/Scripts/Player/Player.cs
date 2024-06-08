using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Player
{
    public class Player : MonoBehaviour, InputActions.IPlayerActions
    {
        private InputActions _inputActions;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _moveInput;
        public float moveSpeed = 10f;
        public float jumpForce = 5f;
        private bool _isGrounded;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputActions();
                _inputActions.Player.SetCallbacks(this);
            }
            _inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            Debug.Log($"Move Input: {_moveInput}");
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && _isGrounded)
            {
                _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump!");
            }
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector2 moveVector = new Vector2(_moveInput.x * moveSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = moveVector;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true;
                Debug.Log("Grounded");
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _isGrounded = false;
                Debug.Log("Not Grounded");
            }
        }
    }
}

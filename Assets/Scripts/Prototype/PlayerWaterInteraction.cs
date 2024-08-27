using UnityEngine;
using UnityEngine.InputSystem;
using Characters;

public class PlayerWaterInteraction : MonoBehaviour
{
    [SerializeField] private float waterSpeedMultiplier = 0.5f; // Player's speed is reduced in water
    [SerializeField] private float waterJumpMultiplier = 1.5f;  // Player's jump height is increased in water
    [SerializeField] private float maxBreathTime = 10f;         // Maximum time the player can hold their breath
    [SerializeField] private float breathDrainRate = 1f;        // Rate at which breath is drained
    [SerializeField] private float breathRegenRate = 2f;        // Rate at which breath is regenerated when out of water

    private float currentBreath;
    private bool isInWater = false;
    private Rigidbody2D rb;
    private MovementController movementController;
    private InputAction moveAction;
    private InputAction jumpAction;

    private float originalMoveSpeed;
    private float originalJumpHeight;
    private float originalGravityScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentBreath = maxBreathTime;
        originalGravityScale = rb.gravityScale;

        // Find and assign the MovementController component
        movementController = GetComponent<MovementController>();

        // Cache original movement and jump values from MovementController
        originalMoveSpeed = movementController.moveSpeed;
        originalJumpHeight = movementController.jumpHeight;

        var playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    private void Update()
    {
        HandleBreath();

        if (isInWater)
        {
            AdjustMovementInWater();
        }
    }

    private void HandleBreath()
    {
        if (isInWater)
        {
            currentBreath -= breathDrainRate * Time.deltaTime;
            if (currentBreath <= 0)
            {
                Debug.Log("Player is out of breath!");
                // Call a method to handle player damage or death
            }
        }
        else
        {
            currentBreath = Mathf.Min(currentBreath + breathRegenRate * Time.deltaTime, maxBreathTime);
        }
    }

    private void AdjustMovementInWater()
    {
        // Adjust movement speed and jump height while in water
        movementController.moveSpeed = originalMoveSpeed * waterSpeedMultiplier;
        movementController.jumpHeight = originalJumpHeight * waterJumpMultiplier;

        Debug.Log($"Movement adjusted in water: Speed={movementController.moveSpeed}, JumpHeight={movementController.jumpHeight}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            EnterWater();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            ExitWater();
        }
    }

    private void EnterWater()
    {
        isInWater = true;
        rb.gravityScale = originalGravityScale * waterJumpMultiplier;
        Debug.Log("Player entered water.");
    }

    private void ExitWater()
    {
        isInWater = false;
        rb.gravityScale = originalGravityScale;

        // Reset movement speed and jump height to their original values
        movementController.moveSpeed = originalMoveSpeed;
        movementController.jumpHeight = originalJumpHeight;

        Debug.Log("Player exited water.");
    }
}

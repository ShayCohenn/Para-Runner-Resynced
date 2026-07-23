using UnityEngine;
using UnityEngine.InputSystem;
using ParaRunner.Core;

namespace ParaRunner.Gameplay
{
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Drag your GameBalanceSO asset here")]
    [SerializeField] private GameBalanceSO gameBalance;

    private CharacterController controller;
    
    // Lane tracking: -1 = Left, 0 = Center, 1 = Right
    private int desiredLane = 0; 
    
    // Vertical physics state
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (gameBalance == null)
        {
            Debug.LogError("GameBalanceSO is not assigned on " + gameObject.name, this);
            return;
        }

        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        // Change Lane Right (D, Right Arrow)
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            if (desiredLane < 1)
            {
                desiredLane++;
            }
        }
        // Change Lane Left (A, Left Arrow)
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            if (desiredLane > -1)
            {
                desiredLane--;
            }
        }

        // Jump (Space, W, Up Arrow)
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (controller.isGrounded)
            {
                verticalVelocity = gameBalance.jumpForce;
            }
        }
    }

    private void MovePlayer()
    {
        // 1. Calculate Target X position based on desired lane and lane distance
        float targetX = desiredLane * gameBalance.laneDistance;
        
        // Calculate horizontal delta movement smoothly
        float currentX = transform.position.x;
        float newX = Mathf.MoveTowards(currentX, targetX, gameBalance.laneChangeSpeed * Time.deltaTime);
        float xMovement = newX - currentX;

        // 2. Handle Gravity & Grounding
        if (controller.isGrounded && verticalVelocity < 0)
        {
            // Small downward snap to keep character firmly grounded
            verticalVelocity = -2f; 
        }
        else
        {
            // Apply gravity over time
            verticalVelocity += gameBalance.gravity * Time.deltaTime;
        }

        // 3. Assemble complete movement vector (Horizontal, Vertical, Forward)
        Vector3 moveVector = new Vector3(
            xMovement / Time.deltaTime, // Converted back to velocity for CharacterController.Move
            verticalVelocity,
            gameBalance.forwardSpeed
        );

        // 4. Apply movement
        controller.Move(moveVector * Time.deltaTime);
    }
}
}
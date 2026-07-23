using UnityEngine;

namespace ParaRunner.Core
{
    [CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "ParaRunner/Game Balance Config")]
    public class GameBalanceSO : ScriptableObject
    {
        [Header("Movement Settings")]
        public float forwardSpeed = 10f;
        public float laneDistance = 3f;
        public float laneChangeSpeed = 15f;

        [Header("Physics & Jump")]
        public float jumpForce = 8f;
        public float gravity = -20f;

        /// <summary>
        /// Handles character movement physics and updates position via CharacterController.
        /// </summary>
        public void MovePlayer(Transform playerTransform, CharacterController controller, int desiredLane, ref float verticalVelocity)
        {
            // 1. Calculate Target X position based on desired lane and lane distance
            float targetX = desiredLane * laneDistance;

            // Calculate horizontal delta movement smoothly
            float currentX = playerTransform.position.x;
            float newX = Mathf.MoveTowards(currentX, targetX, laneChangeSpeed * Time.deltaTime);
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
                verticalVelocity += gravity * Time.deltaTime;
            }

            // 3. Assemble complete movement vector (Horizontal, Vertical, Forward)
            Vector3 moveVector = new Vector3(
                xMovement / Time.deltaTime, // Converted back to velocity for CharacterController.Move
                verticalVelocity,
                forwardSpeed
            );

            // 4. Apply movement via CharacterController
            controller.Move(moveVector * Time.deltaTime);
        }
    }
}
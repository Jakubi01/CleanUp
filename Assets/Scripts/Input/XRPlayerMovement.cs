using UnityEngine;

namespace Input
{
    [RequireComponent(typeof(CharacterController))]
    public class XRPlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform hmd;
        [SerializeField] private XRPlayerInput playerInput;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float moveSpeed = 2f;
        [SerializeField, Min(0f)] private float sprintSpeed = 4f;

        [Header("Jump & Gravity")]
        [SerializeField] private bool enableJump = true;
        [SerializeField, Min(0f)] private float jumpHeight = 1f;
        [SerializeField] private float gravity = -9.81f;

        private CharacterController _characterController;
        private float _verticalVelocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            if (!playerInput)
                playerInput = GetComponent<XRPlayerInput>();

            if (!hmd&& Camera.main)
                hmd = Camera.main.transform;
        }

        private void Update()
        {
            if (!hmd || !playerInput)
                return;

            HandleMovement();
        }

        private void HandleMovement()
        {
            var input = Vector2.ClampMagnitude(playerInput.MoveInput, 1f);

            // HMD의 수평 방향을 기준으로 이동
            var forward = Vector3.ProjectOnPlane(hmd.forward, Vector3.up).normalized;
            var right = Vector3.ProjectOnPlane(hmd.right, Vector3.up).normalized;

            // HMD가 거의 수직 방향을 바라볼 때의 예외 처리
            if (forward.sqrMagnitude < 0.001f)
            {
                forward = Vector3.ProjectOnPlane(
                    transform.forward, Vector3.up).normalized;
            }

            var moveDirection = forward * input.y + right * input.x;
            moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

            var speed = playerInput.IsSprinting
                ? sprintSpeed
                : moveSpeed;

            var velocity = moveDirection * speed;
            
            var grounded = _characterController.isGrounded;
            var jumpPressed = playerInput.ConsumeJump();

            if (grounded)
            {
                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f;

                if (enableJump && jumpPressed)
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            
            _verticalVelocity += gravity * Time.deltaTime;
            velocity.y = _verticalVelocity;
            
            _characterController.Move(velocity * Time.deltaTime);
        }
    }
}
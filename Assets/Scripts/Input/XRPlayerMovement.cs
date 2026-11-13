#if OCULUS
using Oculus.Interaction.Locomotion;
using CharacterController = Oculus.Interaction.Locomotion.CharacterController;
#else
using CharacterController = UnityEngine.CharacterController;
using UnityEngine;
#endif

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
        [SerializeField, Min(0f)] private float crouchSpeed = 1f;

        [Header("Jump & Gravity")]
        [SerializeField] private bool enableJump = true;
        [SerializeField, Min(0f)] private float jumpHeight = 1f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Crouch")]
        [SerializeField] private float standingHeight = 3.0f;
        [SerializeField] private float crouchHeight = 2.0f;
        [SerializeField] private float timeToCrouch = 8f;

        [Header("Camera Rig Offset (for Virtual Crouch)")]
        [SerializeField] private Transform cameraRig;

        private CharacterController _characterController;
        private float _verticalVelocity;
        private float _currentHeightTarget;
        private float _virtualCrouchYOffset;
        private Vector3 _cameraRigInitialLocalPosition;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            if (playerInput == null)
                playerInput = GetComponent<XRPlayerInput>();

            if (hmd == null && Camera.main != null)
                hmd = Camera.main.transform;

            _currentHeightTarget = standingHeight;

            if (cameraRig != null)
                _cameraRigInitialLocalPosition = cameraRig.localPosition;
        }

        private void Update()
        {
            if (hmd is null || playerInput is null)
                return;

            HandleCrouch();
            HandleMovement();
        }

        private void HandleMovement()
        {
            var input = Vector2.ClampMagnitude(playerInput.MoveInput, 1f);
            var forward = Vector3.ProjectOnPlane(hmd.forward, Vector3.up).normalized;
            var right = Vector3.ProjectOnPlane(hmd.right, Vector3.up).normalized;

            if (forward.sqrMagnitude < 0.001f)
            {
                forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            }

            var moveDirection = forward * input.y + right * input.x;
            moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

            float speed = moveSpeed;
            if (playerInput.IsSprinting && !playerInput.IsCrouching)
            {
                speed = sprintSpeed;
            }
            else if (playerInput.IsCrouching)
            {
                speed = crouchSpeed;
            }

            var velocity = moveDirection * speed;

            var grounded = _characterController.isGrounded;
            var jumpPressed = playerInput.ConsumeJump();

            if (grounded)
            {
                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f;

                if (enableJump && jumpPressed && !playerInput.IsCrouching)
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            _verticalVelocity += gravity * Time.deltaTime;
            velocity.y = _verticalVelocity;

            _characterController.Move(velocity * Time.deltaTime);
        }

        private void HandleCrouch()
        {
            var shouldCrouch = playerInput.IsCrouching;

            if (!shouldCrouch && IsCeilingAbove())
            {
                shouldCrouch = true;
            }

            _currentHeightTarget = shouldCrouch ? crouchHeight : standingHeight;
            _characterController.height = Mathf.Lerp(_characterController.height, _currentHeightTarget, Time.deltaTime * timeToCrouch);
            _characterController.center = new Vector3(0f, _characterController.height / 2f, 0f);

            if (cameraRig is null) return;
            var targetOffset = shouldCrouch ? (crouchHeight - standingHeight) * 0.5f : 0f;
            _virtualCrouchYOffset = Mathf.Lerp(_virtualCrouchYOffset, targetOffset, Time.deltaTime * timeToCrouch);

            var rigPos = _cameraRigInitialLocalPosition;
            rigPos.y += _virtualCrouchYOffset;

            cameraRig.localPosition = rigPos;
        }

        private bool IsCeilingAbove()
        {
            var origin = transform.position + Vector3.up * _characterController.height;
            var radius = _characterController.radius - 0.05f;
            var distance = standingHeight - _characterController.height;

            if (distance <= 0.01f) return false;

            return Physics.SphereCast(origin, radius, Vector3.up, out RaycastHit hit, distance);
        }
    }
}

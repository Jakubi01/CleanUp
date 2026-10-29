using Assets.Scripts;
using UnityEngine;

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

        if (playerInput == null)
            playerInput = GetComponent<XRPlayerInput>();

        if (hmd == null && Camera.main != null)
            hmd = Camera.main.transform;
    }

    private void Update()
    {
        if (hmd == null || playerInput == null)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = Vector2.ClampMagnitude(
            playerInput.MoveInput, 1f);

        // HMD의 수평 방향을 기준으로 이동한다.
        Vector3 forward = Vector3.ProjectOnPlane(
            hmd.forward, Vector3.up).normalized;

        Vector3 right = Vector3.ProjectOnPlane(
            hmd.right, Vector3.up).normalized;

        // HMD가 거의 수직 방향을 바라볼 때의 예외 처리
        if (forward.sqrMagnitude < 0.001f)
        {
            forward = Vector3.ProjectOnPlane(
                transform.forward, Vector3.up).normalized;
        }

        Vector3 moveDirection =
            forward * input.y + right * input.x;

        moveDirection = Vector3.ClampMagnitude(
            moveDirection, 1f);

        float speed = playerInput.IsSprinting
            ? sprintSpeed
            : moveSpeed;

        Vector3 velocity = moveDirection * speed;

        // 착지 및 점프 처리
        bool grounded = _characterController.isGrounded;

        bool jumpPressed = playerInput.ConsumeJump();

        if (grounded)
        {
            if (_verticalVelocity < 0f)
                _verticalVelocity = -2f;

            if (enableJump && jumpPressed)
            {
                _verticalVelocity = Mathf.Sqrt(
                    jumpHeight * -2f * gravity);
            }
        }

        // 중력 적용
        _verticalVelocity += gravity * Time.deltaTime;
        velocity.y = _verticalVelocity;

        // 이동 및 충돌 처리
        _characterController.Move(
            velocity * Time.deltaTime);
    }
}
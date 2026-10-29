using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class XRPlayerInput : MonoBehaviour
    {
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference lookAction;
        [SerializeField] private InputActionReference interactAction;
        [SerializeField] private InputActionReference crouchAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference actionAction;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        public bool IsCrouching { get; private set; }
        public bool IsSprinting { get; private set; }

        public bool LookIsMouseDelta { get; private set; }

        private bool _interactPressed;
        private bool _jumpPressed;
        private bool _actionPressed;

        private void OnEnable()
        {
            Bind(moveAction, OnMove);
            Bind(lookAction, OnLook);
            Bind(interactAction, OnInteract);
            Bind(crouchAction, OnCrouch);
            Bind(jumpAction, OnJump);
            Bind(sprintAction, OnSprint);
            Bind(actionAction, OnAction);
        }

        private void OnDisable()
        {
            Unbind(moveAction, OnMove);
            Unbind(lookAction, OnLook);
            Unbind(interactAction, OnInteract);
            Unbind(crouchAction, OnCrouch);
            Unbind(jumpAction, OnJump);
            Unbind(sprintAction, OnSprint);
            Unbind(actionAction, OnAction);

            MoveInput = Vector2.zero;
            LookInput = Vector2.zero;

            IsCrouching = false;
            IsSprinting = false;

            _interactPressed = false;
            _jumpPressed = false;
            _actionPressed = false;
        }

        private void Bind(
            InputActionReference reference,
            System.Action<InputAction.CallbackContext> callback)
        {
            if (reference == null || reference.action == null)
                return;

            InputAction action = reference.action;

            action.performed += callback;
            action.canceled += callback;
            action.Enable();
        }

        private void Unbind(
            InputActionReference reference,
            System.Action<InputAction.CallbackContext> callback)
        {
            if (reference == null || reference.action == null)
                return;

            InputAction action = reference.action;

            action.performed -= callback;
            action.canceled -= callback;
            action.Disable();
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.canceled
                ? Vector2.zero
                : ctx.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            if (ctx.canceled)
            {
                LookInput = Vector2.zero;
                return;
            }

            LookInput = ctx.ReadValue<Vector2>();

            LookIsMouseDelta = ctx.control != null &&
                               ctx.control.device is Mouse;
        }

        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                _interactPressed = true;
        }

        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            IsCrouching = !ctx.canceled &&
                          ctx.ReadValueAsButton();
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                _jumpPressed = true;
        }

        public void OnSprint(InputAction.CallbackContext ctx)
        {
            IsSprinting = !ctx.canceled &&
                          ctx.ReadValueAsButton();
        }

        public void OnAction(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                _actionPressed = true;
        }

        public bool ConsumeInteract()
        {
            bool pressed = _interactPressed;
            _interactPressed = false;
            return pressed;
        }

        public bool ConsumeJump()
        {
            bool pressed = _jumpPressed;
            _jumpPressed = false;
            return pressed;
        }

        public bool ConsumeAction()
        {
            bool pressed = _actionPressed;
            _actionPressed = false;
            return pressed;
        }

        private void LateUpdate()
        {
            // 마우스 델타는 프레임마다 새 입력을 받아야 한다.
            if (LookIsMouseDelta)
                LookInput = Vector2.zero;
        }
    }
}
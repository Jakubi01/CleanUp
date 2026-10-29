using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private XRPlayerInput playerInput;
    [SerializeField] private Transform hmd;

    [Header("Mouse Look")]
    [SerializeField, Min(0f)] private float mouseSensitivity = 0.1f;
    [SerializeField, Min(0f)] private float stickSensitivity = 120f;
    [SerializeField] private float minPitch = -85f;
    [SerializeField] private float maxPitch = 85f;

    private float _pitch;
    private bool _initialized;
    private Quaternion _initialHmdLocalRotation;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<XRPlayerInput>();

        if (hmd == null && Camera.main != null)
            hmd = Camera.main.transform;

        if (hmd != null)
        {
            _initialHmdLocalRotation = hmd.localRotation;
            _initialized = true;
        }
    }

    private void Update()
    {
        if (playerInput == null || hmd == null || !_initialized)
            return;

        // XR 기기가 활성화되어 있으면 HMD 추적을 그대로 사용한다.
        if (XRSettings.isDeviceActive)
            return;

        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        Vector2 lookInput = playerInput.LookInput;

        if (playerInput.LookIsMouseDelta)
        {
            // 마우스 델타는 프레임 동안의 이동량이다.
            transform.Rotate(
                Vector3.up,
                lookInput.x * mouseSensitivity,
                Space.World);

            _pitch -= lookInput.y * mouseSensitivity;
        }
        else
        {
            // 스틱 입력은 시간에 비례해 회전시킨다.
            transform.Rotate(
                Vector3.up,
                lookInput.x * stickSensitivity * Time.deltaTime,
                Space.World);

            _pitch -= lookInput.y * stickSensitivity *
                      Time.deltaTime;
        }

        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

        hmd.localRotation =
            _initialHmdLocalRotation *
            Quaternion.Euler(_pitch, 0f, 0f);
    }
}
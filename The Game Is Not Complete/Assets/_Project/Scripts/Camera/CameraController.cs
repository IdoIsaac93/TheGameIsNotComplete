using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.InputSystem.OnScreen;

public class CameraController : Singleton<CameraController>
{
    [Header("Components")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private InputReader inputReader;

    [Header("UI Controls")]
    [SerializeField] private OnScreenStick leftStick;  // Left stick for movement
    [SerializeField] private OnScreenStick rightStick; // Right stick for zoom

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 60f;
    private float zoomLevel = 0f;

    private void OnEnable()
    {
#if UNITY_EDITOR && !UNITY_ANDROID
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
        if (inputReader != null)
        {
            inputReader.Move += OnMove;
            inputReader.Zoom += ZoomCamera;
        }
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.Move -= OnMove;
            inputReader.Zoom -= ZoomCamera;
        }
    }

    private void Start()
    {
        if (cinemachineCamera != null)
            zoomLevel = cinemachineCamera.Lens.FieldOfView;
        else
            Debug.LogError("CinemachineCamera not assigned in CameraController!");

        if (leftStick == null) Debug.LogWarning("Left Stick not assigned!");
        if (rightStick == null) Debug.LogWarning("Right Stick not assigned!");
    }

    private void Update()
    {
        // Update inputs from sticks using predefined control paths
        moveInput = InputSystem.GetDevice<Gamepad>()?.leftStick.ReadValue() ?? Vector2.zero;
        float zoomInput = InputSystem.GetDevice<Gamepad>()?.rightStick.ReadValue().y ?? 0f;

        ZoomCamera(zoomInput);
        MoveCamera();
    }

    private void OnMove(Vector2 input)
    {
        moveInput = input; // Still supports InputReader if used
    }

    private void MoveCamera()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    private void ZoomCamera(float zoomInput)
    {
        if (cinemachineCamera == null) return;

        zoomLevel = Mathf.Clamp(
            zoomLevel - (zoomInput * zoomSpeed * Time.deltaTime),
            minFOV,
            maxFOV
        );

        cinemachineCamera.Lens.FieldOfView = zoomLevel;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
public class CameraController : Singleton<CameraController>
{
    [Header("Components")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private InputReader inputReader;

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

    private void Update()
    {
        MoveCamera();
    }

    private void OnMove(Vector2 input)
    {
        moveInput = input;
    }

    private void MoveCamera()
    {
        Vector3 moveDirection = new (moveInput.x, 0, moveInput.y);
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    private void ZoomCamera(float zoomInput)
    {
        if (cinemachineCamera == null) return;

        zoomLevel = Mathf.Clamp(
            cinemachineCamera.Lens.FieldOfView - (zoomInput * zoomSpeed),
            minFOV,
            maxFOV
        );

        cinemachineCamera.Lens.FieldOfView = zoomLevel;
    }
}
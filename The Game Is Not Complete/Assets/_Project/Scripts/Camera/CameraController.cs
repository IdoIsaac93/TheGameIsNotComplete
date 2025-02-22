using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    //[SerializeField] private CinemachineCamera freeLookCam;
    //[SerializeField] private Transform playerTransform;

    //"New" keyword because base class has an awake method and we want to run both, not override it
    new private void Awake()
    {
        /*
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerTransform != null) { return; }
        playerTransform = GameObject.FindWithTag("Player").transform;
        */
    }

    private void OnEnable()
    {
        //freeLookCam.Target.TrackingTarget = playerTransform;
    }
}
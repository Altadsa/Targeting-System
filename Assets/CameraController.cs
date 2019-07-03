using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    FreeCamera _freeCam;
    TargetingCamera _zCam;
    FirstPersonCamera _fCam;

    private void Awake()
    {
        _freeCam = GetComponent<FreeCamera>();
        _zCam = GetComponent<TargetingCamera>();
        _fCam = GetComponent<FirstPersonCamera>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _zCam.enabled = true;
            _freeCam.enabled = false;
        }
        else
        {
            _zCam.enabled = false;
            _freeCam.enabled = true;
        }
    }

}

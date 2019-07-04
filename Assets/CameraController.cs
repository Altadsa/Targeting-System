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
        if (Input.GetMouseButtonDown(1))
        {
            if (!_fCam.enabled)
                EnableFirstPerson();
            else
                DisableFirstPerson();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _zCam.enabled = true;
            _freeCam.enabled = false;
            _fCam.enabled = false;
        }
        else
        {
            if (!_fCam.enabled)
            {
                _zCam.enabled = false;
                _freeCam.enabled = true;
            }
        }
    }

    private void EnableFirstPerson()
    {
        _fCam.enabled = true;
        _freeCam.enabled = false;
    }

    private void DisableFirstPerson()
    {
        _fCam.enabled = false;
        _freeCam.enabled = true;
    }
    

}

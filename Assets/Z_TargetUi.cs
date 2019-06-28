using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z_TargetUi : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Z_TargetLockon.HasTarget)
        {
            transform.LookAt(_mainCamera.transform);
            transform.position = Z_TargetLockon.Target.position + Vector3.up * 2;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10;

    Camera _mainCamera;
    private float x, z;

    private Vector3 CameraMovement => _mainCamera.ScaledForward() * z + _mainCamera.ScaledRight() * x;

    private Vector3 ForwardMovement => transform.forward * z + transform.right * x;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        if (Z_TargetLockon.HasTarget)
        {
            GetComponent<Animator>().SetBool("HasTarget", true);
            transform.forward = GetTargetForward();
            if (z < 0) z /= 2;
            transform.position += ForwardMovement * moveSpeed * Time.deltaTime;
        }
        else
        {
            GetComponent<Animator>().SetBool("HasTarget", false);
            GetComponent<Animator>().SetFloat("MoveForce", Mathf.Abs(x) + Mathf.Abs(z));
            transform.position += CameraMovement * moveSpeed * Time.deltaTime;
        }

    }

    private Vector3 GetTargetForward()
    {
        var newForward = Z_TargetLockon.Target.transform.position - transform.position;
        newForward.y = 0;
        newForward.Normalize();
        return newForward;
    }

}
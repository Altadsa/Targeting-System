﻿using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public TargetingCamera _targetingCamera;
    public float moveSpeed = 10;
    public Transform Model;

    public Vector3 SmoothVelocity;
    public float SmoothTime;

    Animator _animator;
    Camera _mainCamera;
    private float x, z;

    private Vector3 CameraMovement => _mainCamera.ScaledForward() * z + _mainCamera.ScaledRight() * x;
    Vector3 MoveSpeed => CameraMovement.normalized * moveSpeed * Time.deltaTime;
    private Vector3 _directionToFace;
    private bool HasInput => Mathf.Abs(x) > Mathf.Epsilon || Mathf.Abs(z) > Mathf.Epsilon;

    private bool _canMove = true;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _mainCamera = Camera.main;
    }

    bool _isRolling = false;

    private void Update()
    {
        //Get Inputs from controller
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        z = z < 0 ? z / 2 : z;

        //Determine direction for Player to face
        _directionToFace = HasInput ? CameraMovement.normalized : transform.forward;

        if (_canMove)
        {
            if (_targetingCamera.enabled)
            {
                _animator.SetBool("HasTarget", true);
                _animator.SetFloat("Directîon", x);
                transform.forward = Vector3.Scale(GetTargetForward(), new Vector3(1, 0, 1).normalized);
                Model.forward = GetTargetForward();
            }
            else
            {
                _animator.SetBool("HasTarget", false);

                Model.forward = HasInput ? CameraMovement.normalized : Model.forward;
                transform.forward = Vector3.SmoothDamp(transform.forward, _directionToFace, ref SmoothVelocity, SmoothTime);
            }

            if (Input.GetMouseButtonDown(1))
            {
                _animator.SetBool("Block", true);
            }

            if (Input.GetMouseButtonUp(1))
            {
                _animator.SetBool("Block", false);
            }

            if (!_animator.GetBool("Block"))
            {
                _animator.SetFloat("MoveForce", Mathf.Abs(x) + Mathf.Abs(z * 2));
            }

            if (Input.GetMouseButtonDown(0)) _animator.SetTrigger("Attack");

            if (_animator.GetFloat("MoveForce") > Mathf.Epsilon)
            {
                transform.position += MoveSpeed;
            }

        }
        else
        {
            transform.forward = Vector3.SmoothDamp(transform.forward, _directionToFace, ref SmoothVelocity, SmoothTime);
        }
    }

    public void EnableMovement()
    {
        var camForward = _mainCamera.ScaledForward();
        transform.forward = camForward;
        Model.forward = transform.forward;
        _canMove = true;

    }

    public void DisableMovement()
    {
        var camForward = _mainCamera.ScaledForward();
        transform.forward = camForward;
        Model.forward = transform.forward;
        _canMove = false;
    }

    private Vector3 GetTargetForward()
    {
        if (!_targetingCamera.Target) return transform.forward;
        var newForward = _targetingCamera.Target.position - transform.position;
        newForward.y = 0;
        newForward.Normalize();
        return newForward ;
    }

}
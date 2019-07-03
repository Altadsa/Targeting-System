using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 10;
    public Transform Model;

    Camera _mainCamera;
    private float x, z;

    private Vector3 CameraMovement => _mainCamera.ScaledForward() * z + _mainCamera.ScaledRight() * x;

    private bool HasInput => Mathf.Abs(x) > Mathf.Epsilon || Mathf.Abs(z) > Mathf.Epsilon;

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
            Model.forward = Vector3.Scale(GetTargetForward(), new Vector3(1,0,1).normalized);
        }
        else
        {
            GetComponent<Animator>().SetBool("HasTarget", false);
            GetComponent<Animator>().SetFloat("MoveForce", Mathf.Abs(x) + Mathf.Abs(z));
            Model.forward = transform.forward;
        }
        if (z < 0) z /= 2;
        
        transform.position += CameraMovement * moveSpeed * Time.deltaTime;
        var newfacingDir = HasInput ? CameraMovement.normalized : transform.forward;
        transform.forward = Vector3.Lerp(transform.forward, newfacingDir, Time.deltaTime);
    }

    private Vector3 GetTargetForward()
    {
        var newForward = Z_TargetLockon.Target.transform.position - transform.position;
        newForward.y = 0;
        newForward.Normalize();
        return newForward;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(Model.position, Model.forward * 5f);
    }

}
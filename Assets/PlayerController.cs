using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public TargetingCamera _targetingCamera;
    public float moveSpeed = 10;
    public Transform Model;

    Camera _mainCamera;
    private float x, z;

    private Vector3 CameraMovement => _mainCamera.ScaledForward() * z + _mainCamera.ScaledRight() * x;
    Vector3 MoveSpeed => CameraMovement.normalized * moveSpeed * Time.deltaTime;
    private Vector3 _directionToFace;
    private bool HasInput => Mathf.Abs(x) > Mathf.Epsilon || Mathf.Abs(z) > Mathf.Epsilon;

    private bool _canMove = true;

    private void Awake()
    {
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
                GetComponent<Animator>().SetBool("HasTarget", true);
                transform.forward = Vector3.Scale(GetTargetForward(), new Vector3(1, 0, 1).normalized);
                Model.forward = GetTargetForward();
            }
            else
            {
                GetComponent<Animator>().SetBool("HasTarget", false);
                GetComponent<Animator>().SetFloat("MoveForce", Mathf.Abs(x) + Mathf.Abs(z));
                Model.forward = HasInput ? CameraMovement.normalized : Model.forward;
                transform.forward = Vector3.Lerp(transform.forward, _directionToFace, Time.deltaTime);
            }
            transform.position += MoveSpeed;
        }
        else
        {
            transform.forward = Vector3.Lerp(transform.forward, _directionToFace, Time.deltaTime);
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
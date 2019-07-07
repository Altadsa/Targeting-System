using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    public TargetingCamera _targetingCamera;
    public float moveSpeed = 10;
    public Transform Model;

    Camera _mainCamera;
    private float x, z;

    private Vector3 CameraMovement => _mainCamera.ScaledForward() * z + _mainCamera.ScaledRight() * x;
    private Vector3 _directionToFace;
    private bool HasInput => Mathf.Abs(x) > Mathf.Epsilon || Mathf.Abs(z) > Mathf.Epsilon;

    private bool _canMove = true;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        //Get Inputs from controller
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        z = z < 0 ? z / 2 : z;

        //Determine direction for Player to face
        _directionToFace = HasInput ? CameraMovement.normalized : transform.forward;


        if (_targetingCamera.enabled)
        {
            GetComponent<Animator>().SetBool("HasTarget", true);
            transform.forward = Vector3.Scale(GetTargetForward(), new Vector3(1, 0, 1).normalized);
        }
        else
        {
            GetComponent<Animator>().SetBool("HasTarget", false);
            GetComponent<Animator>().SetFloat("MoveForce", Mathf.Abs(x) + Mathf.Abs(z));
            Model.forward = HasInput ? CameraMovement.normalized : Model.forward;
            transform.forward = Vector3.Lerp(transform.forward, _directionToFace, Time.deltaTime);
        }


        if (_canMove)
        {
            transform.position += CameraMovement.normalized * moveSpeed * Time.deltaTime;
        }
 
    }


    private Vector3 GetTargetForward()
    {
        if (!_targetingCamera.Target) return transform.forward;
        var newForward = _targetingCamera.Target.position - transform.position;
        newForward.y = 0;
        newForward.Normalize();
        return newForward ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 10);
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(Model.position, Model.forward * 10);
    }

}
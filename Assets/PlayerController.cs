using UnityEngine;


public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 10;
    public Transform Model;

    Camera _mainCamera;
    private float x, z;

    private Vector3 CameraMovement => _mainCamera.ScaledForward() * z + _mainCamera.ScaledRight() * x;

    private bool HasInput => Mathf.Abs(x) > Mathf.Epsilon || Mathf.Abs(z) > Mathf.Epsilon;

    private bool _canMove = true;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        var newfacingDir = HasInput ? CameraMovement.normalized : transform.forward;
        if (Z_TargetLockon.HasTarget)
        {
            GetComponent<Animator>().SetBool("HasTarget", true);
            transform.forward = Vector3.Scale(GetTargetForward(), new Vector3(1,0,1).normalized);
            Model.forward = GetTargetForward();
        }
        else
        {
            GetComponent<Animator>().SetBool("HasTarget", false);
            GetComponent<Animator>().SetFloat("MoveForce", Mathf.Abs(x) + Mathf.Abs(z));
            if (_canMove)
                Model.forward = HasInput ? CameraMovement.normalized : Model.forward;
        }
        if (z < 0) z /= 2;
        if (_canMove)
        {
            transform.position += CameraMovement * moveSpeed * Time.deltaTime;
        }
        transform.forward = Vector3.Lerp(transform.forward, newfacingDir, Time.deltaTime);
    }

    public void AllowMovement()
    {
        _canMove = true;
        transform.forward = _mainCamera.ScaledForward();
        Model.forward = _mainCamera.ScaledForward();
    }

    public void DisableMovement()
    {
        _canMove = false;
        transform.forward = Model.forward;
        Model.forward = transform.forward;
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
        Gizmos.DrawRay(transform.position, transform.forward * 10);
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(Model.position, Model.forward * 10);
    }

}
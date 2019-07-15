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
    Vector3 MoveSpeed => (CameraMovement.normalized * moveSpeed+ Physics.gravity) * Time.deltaTime ;
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

        if (Input.GetKeyDown(KeyCode.Space) && !_isRolling)
        {
            StartCoroutine(Roll());
        }

        if (_canMove && !_isRolling)
        {
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
          //  transform.position += MoveSpeed;
        }
        else
        {
            transform.forward = Vector3.Lerp(transform.forward, _directionToFace, Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        //rb.AddForce(CameraMovement.normalized * moveSpeed, ForceMode.Acceleration);
        if (HasInput)
            rb.velocity = MoveSpeed;
        else
            rb.velocity = Physics.gravity;
    }

    IEnumerator Roll()
    {
        _isRolling = true;
        var sT = Time.time;
        var newPos = transform.position + Model.forward * 5f;
        while (Time.time - sT < 0.5f)
        {
            var delta = Time.time - sT;
            delta /= 0.5f;
            if (delta > 1)
            {
                delta = 1;
            }

            rb.position = Vector3.Lerp(rb.position,newPos, delta);
            yield return new WaitForEndOfFrame();
        }

        _isRolling = false;
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
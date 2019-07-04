using System.Collections;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    public float Speed = 10;
    [SerializeField] Transform _fpView;
    PlayerController _player;

    const float MAX_VERTICAL = 75f;

    float  _verticalThrow;
    float _moveStart = 0;
    float _moveDuration = 1;
    float _verticalAngle = 0;
    bool _canMoveCamera = false;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        StartCoroutine(MoveToView());
    }

    IEnumerator MoveToView()
    {
        _player.CanMove = false;
        _moveStart = Time.time;
        while (Time.time - _moveStart < _moveDuration)
        {
            var delta = Time.time - _moveStart;
            delta /= _moveDuration;
            if (delta > 1)
            {
                delta = 1;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, delta);
            transform.position = Vector3.Lerp(transform.position, _fpView.position, delta);
            yield return new WaitForEndOfFrame();
        }
        _canMoveCamera = true;
    }

    private void OnDisable()
    {
        _player.CanMove = true;
    }

    private void Update()
    {
        _verticalThrow = -Input.GetAxisRaw("Vertical");
        if (_canMoveCamera)
        {
            _verticalAngle += _verticalThrow * Speed * Time.deltaTime;
            var horizontalRot = Quaternion.AngleAxis(_player.transform.eulerAngles.y, Vector3.up);
            var verticalRot = Quaternion.AngleAxis(_verticalAngle, Vector3.right);
            transform.rotation = horizontalRot * verticalRot;
        }

    }

    private void LateUpdate()
    {
        transform.position = _fpView.position;
    }

}

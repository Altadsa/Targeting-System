using UnityEngine;

public class FirstPersonCamera : PlayerCamera
{

    public float Speed = 10;
    [SerializeField] Transform _fpView;
    [SerializeField] UiHookshot _hookshotUi;
    PlayerController _player;

    const float MAX_VERTICAL = 75f;

    float  _verticalThrow, _horizontalThrow;
    float _verticalAngle = 0, _horizontalAngle = 0;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        StartCoroutine(MoveToPosition(_fpView ,_player));
        //_player.DisableMovement();
        _hookshotUi.ShowUi();
    }

    private void OnDisable()
    {
        _player.EnableMovement();
        _hookshotUi.HideUi();
        _verticalAngle = 0;
    }

    private void Update()
    {
        _verticalThrow = -Input.GetAxisRaw("Vertical");
        if (_inPosition)
        {
            _verticalAngle = transform.eulerAngles.x + _verticalThrow * Speed * Time.deltaTime;
            _horizontalAngle = transform.eulerAngles.y + _horizontalThrow * Speed * Time.deltaTime;
            var horizontalRot = Quaternion.AngleAxis(_player.transform.eulerAngles.y, Vector3.up);
            var verticalRot = Quaternion.AngleAxis(_verticalAngle, Vector3.right);
            transform.rotation = horizontalRot * verticalRot;
        }
    }

    private void LateUpdate()
    {
        if (_inPosition)
        {
            transform.position = _fpView.position;
        }

    }

}

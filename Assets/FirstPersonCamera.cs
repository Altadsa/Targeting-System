﻿using UnityEngine;

public class FirstPersonCamera : PlayerCamera
{

    public float Speed = 10;
    [SerializeField] Transform _fpView;
    PlayerController _player;

    const float MAX_VERTICAL = 75f;

    float  _verticalThrow;
    float _verticalAngle = 0;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        _player.CanMove = false;
        StartCoroutine(MoveToPosition(_fpView.position));
    }

    private void OnDisable()
    {
        _player.CanMove = true;
    }

    private void Update()
    {
        _verticalThrow = -Input.GetAxisRaw("Vertical");
        if (_inPosition)
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

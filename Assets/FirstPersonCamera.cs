using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    float x, z;
    public float Speed = 10;
    [SerializeField] Transform _fpView;
    PlayerController _player;

    const float MAX_VERTICAL = 75f;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        StartCoroutine(MoveToView());
    }

    float sT = 0;
    float duration = 1;
    IEnumerator MoveToView()
    {
        sT = Time.time;
        while (Time.time - sT < duration)
        {
            var delta = Time.time - sT;
            delta /= duration;
            if (delta > 1)
            {
                delta = 1;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, delta);
            transform.position = Vector3.Lerp(transform.position, _fpView.position, delta);
            yield return new WaitForEndOfFrame();
        }
        _player.CanMove = false;
        _canMoveCamera = true;
    }

    private void OnDisable()
    {
        _player.CanMove = true;
    }

    bool _canMoveCamera = false;

    float xAngle = 0;
    float zAngle = 0;

    private void Update()
    {
        z = -Input.GetAxisRaw("Vertical");
        if (_canMoveCamera)
        {
            zAngle += z * Speed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(_player.transform.eulerAngles.y, Vector3.up) *  Quaternion.AngleAxis(zAngle, Vector3.right);
            ClampVerticalView();
        }

    }

    private void ClampVerticalView()
    {
        var euler = transform.eulerAngles;
        euler.x = Mathf.Clamp(euler.x, -MAX_VERTICAL, MAX_VERTICAL);
        transform.eulerAngles = euler;
    }

    private void LateUpdate()
    {
        transform.position = _fpView.position;
    }

}

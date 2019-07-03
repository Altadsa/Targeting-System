using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    float x, z;
    public float Speed = 10;
    [SerializeField] Transform _fpView;
    PlayerController _player;

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

            transform.position = Vector3.Lerp(transform.position, _fpView.position, delta);
            yield return new WaitForEndOfFrame();
        }
        _player.CanMove = false;
        _canMoveCamera = true;
        transform.rotation = Quaternion.identity;
        Debug.Log("End Reached;");
    }

    private void OnDisable()
    {
        
    }

    bool _canMoveCamera = false;

    float xAngle = 0;
    float zAngle = 0;

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        if (_canMoveCamera)
        {
            xAngle += x * Speed * Time.deltaTime;
            zAngle += z * Speed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(xAngle, Vector3.up) * Quaternion.AngleAxis(zAngle, Vector3.right);
        }


    }

}

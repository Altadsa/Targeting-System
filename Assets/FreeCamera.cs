using System.Collections;
using UnityEngine;

public class FreeCamera : PlayerCamera
{
    //Transform of the Player
    public Rigidbody Player;
    public Transform Model;
    //Free Camera PositionOffset
    public Vector3 PositionOffset;
    //Distance behind the Player to move to
    public float MaxDistance = 3f;

    float LerpSmoothing => Time.deltaTime * 5;

    //Property to get the desired position relative to the Player
    private Vector3 CameraPosition => Player.transform.position - Vector3.Scale( (Player.transform.position - transform.position), new Vector3(1,0,1)).normalized * MaxDistance + PositionOffset;

    Vector3 InitialPosition => Player.transform.position - Model.forward * MaxDistance + PositionOffset;

    RaycastHit hit;

    private void OnEnable()
    {
        StartCoroutine(MoveToPosition(CheckForCollision(Player.transform.position,InitialPosition)));
    }

   Vector3 _velocity = Vector3.one; 
    private void LateUpdate()
    {
        var lookDirection = (Player.transform.position - transform.position);
        transform.forward = Vector3.Lerp(transform.forward, lookDirection.normalized, LerpSmoothing);
        if (_inPosition)
        {
            var newPosition = CheckForCollision(Player.transform.position, CameraPosition);
            transform.position = Vector3.Lerp(transform.position, newPosition, LerpSmoothing);

        }

    }



}
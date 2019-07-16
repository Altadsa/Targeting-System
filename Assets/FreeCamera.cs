using System.Collections;
using UnityEngine;

public class FreeCamera : PlayerCamera
{
    //Transform of the Player
    public Transform Player;
    //Free Camera PositionOffset
    public Vector3 PositionOffset;
    //Distance behind the Player to move to
    public float MaxDistance = 3f;

    float LerpSmoothing => Time.deltaTime * 5;

    //Property to get the desired position relative to the Player
    private Vector3 CameraPosition => Player.position - Vector3.Scale( (Player.position - transform.position), new Vector3(1,0,1)).normalized * MaxDistance + PositionOffset;

    Vector3 InitialPosition => Player.position - Player.forward * MaxDistance + PositionOffset;

    private void OnEnable()
    {
        StartCoroutine(MoveToPosition(CheckForCollision(Player.position,InitialPosition)));
    }

   Vector3 _velocity = Vector3.one; 
    private void LateUpdate()
    {
        var lookDirection = Player.position - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, lookDirection.normalized, LerpSmoothing);
        if (_inPosition)
        {
            var newPosition = CheckForCollision(Player.position, CameraPosition);
            transform.position = Vector3.Lerp(transform.position, newPosition, LerpSmoothing);
        }

    }



}
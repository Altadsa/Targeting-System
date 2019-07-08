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

    //Property to get the desired position relative to the Player
    private Vector3 CameraPosition => Player.position - Vector3.Scale( (Player.position - transform.position), new Vector3(1,0,1)).normalized * MaxDistance + PositionOffset;

    Vector3 InitialPosition => Player.position - Player.forward * MaxDistance + PositionOffset;

    RaycastHit hit;

    private void OnEnable()
    {
        StartCoroutine(MoveToPosition(InitialPosition));
    }

    private void LateUpdate()
    {
        var lookDirection = (Player.position - transform.position);
        transform.forward = Vector3.Lerp(transform.forward, lookDirection.normalized, Time.deltaTime * 5f);
        if (_inPosition)
        {
            var newPosition = CheckForCollision(Player.position, CameraPosition);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 5f);
        }

    }



}
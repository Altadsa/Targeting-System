using System.Collections;
using UnityEngine;

public class FreeCamera : PlayerCamera
{
    //Transform of the Player
    public Transform Player;
    //Free Camera PositionOffset
    public Vector3 PositionOffset;
    //LookAt Position Offset
    public Vector3 LookAtOffset;
    //Distance behind the Player to move to
    public float MaxDistance = 3f;

    //Property to get the desired position behind the Player
    private Vector3 CameraPosition => Player.position - Player.forward * MaxDistance + PositionOffset;

    private void OnEnable()
    {
        StartCoroutine(MoveToPosition(CameraPosition));
    }

    private void LateUpdate()
    {
        transform.LookAt(Player.position + LookAtOffset);
        if (_inPosition)
        {
            var distance = Vector3.Distance(transform.position, CameraPosition);
            transform.position = Vector3.Lerp(transform.position, CameraPosition, Time.deltaTime * distance);
        }
    }

}
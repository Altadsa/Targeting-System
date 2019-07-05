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

    public LayerMask LayerMask;

    //Property to get the desired position behind the Player
    private Vector3 CameraPosition => Player.position - Player.forward * MaxDistance + PositionOffset;

    private void OnEnable()
    {
        StartCoroutine(MoveToPosition(CameraPosition));
    }


    RaycastHit hit;
    private void LateUpdate()
    {
        transform.LookAt(Player.position + LookAtOffset);
        if (_inPosition)
        {
            var distance = Vector3.Distance(transform.position, CameraPosition);
            bool colliding = Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance, ~(LayerMask));
            if (colliding) Debug.Log(hit.collider.gameObject.name);
            var movePosition = colliding ? hit.point : CameraPosition;
            transform.position = Vector3.Lerp(transform.position, movePosition, Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward*MaxDistance);
    }

}
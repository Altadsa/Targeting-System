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
    //Layers to Ignore when checking for Collision
    public LayerMask LayerMask;


    //Property to get the desired position relative to the Player
    private Vector3 CameraPosition => Player.position - Vector3.Scale( (Player.position - transform.position), new Vector3(1,0,1)).normalized * MaxDistance + PositionOffset;

    RaycastHit hit;
    private void LateUpdate()
    {
        var lookDirection = (Player.position - transform.position);
        transform.forward = Vector3.Lerp(transform.forward, lookDirection.normalized, Time.deltaTime * 5f);
        var newPosition = CheckForCollision(CameraPosition);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 5f);
    }

    private Vector3 CheckForCollision(Vector3 cameraPosition)
    {
        var direction = cameraPosition - Player.position;
        bool colliding = Physics.Raycast(Player.position, direction.normalized, out hit, direction.magnitude, ~(LayerMask));
        if (colliding) Debug.Log($"Hit {hit.collider.gameObject.name}");
        return colliding ? hit.point : CameraPosition;
    }

}
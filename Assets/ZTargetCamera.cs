using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTargetCamera : MonoBehaviour
{
    public Transform Player;
    public Transform Target;
    public Camera Main;

    private const float OFFSET = 25f;

    private Vector3 midepoint;
    private float distToMidePoint;
    private float distBetTargs;
    private float cameraDistance;
    private Vector3 playerScreenPosition;
    private float rotationTime = 1;
    private float timeElapsed = 0;
    public float TargetingDistance;

    private bool dir;

    private Vector3 targetsVect;

    private void Update()
    {
        playerScreenPosition = Main.WorldToViewportPoint(Player.position);
        var screendistance = new Vector3(0.5f, 0.5f, 0) - playerScreenPosition;
        if (Mathf.Abs(screendistance.x) > .125f || Mathf.Abs(screendistance.y) > .25f)
        {
            transform.forward = Vector3.Lerp(transform.forward, Player.forward, Time.deltaTime);

        }

        dir = Vector3.SignedAngle(transform.position,midepoint, Vector3.up) > Mathf.Epsilon;
        targetsVect = Target.position - Player.position;
        midepoint = Player.position + 0.5f * targetsVect + Vector3.up;
        Main.transform.LookAt(midepoint);

    }

    private void LateUpdate()
    {
        var rot = !dir ? 30 : -30;
        var rotFor = Quaternion.AngleAxis(rot, Vector3.up) * -targetsVect.normalized;
        transform.position = Vector3.Lerp(transform.position, Player.position + rotFor * TargetingDistance, Time.deltaTime);
        //transform.forward = Vector3.Lerp( transform.forward ,Vector3.Scale(midepoint - transform.position, new Vector3(1, 0, 1)).normalized, Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(midepoint,  - Player.forward * TargetingDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
        Gizmos.DrawRay(Player.position, Player.forward * 5f);


    }

}

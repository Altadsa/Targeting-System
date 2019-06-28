using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTargetCamera : MonoBehaviour
{
    public Transform Player;
    public Transform Target;
    public Camera Main;

    private const float DISTANCE_MARGIN = 3;

    private Vector3 midepoint;
    private float distToMidePoint;
    private float distBetTargs;
    private float cameraDistance;
    private float aspectRatio;
    private float tanFov;

    private Vector3 playerScreenPosition;
    private float rotationTime = 1;
    private float timeElapsed = 0;

    private void Start()
    {
        aspectRatio = Screen.width / Screen.height;
        tanFov = Mathf.Tan(Mathf.Deg2Rad * Main.fieldOfView / 2.0f);
    }

    private void Update()
    {
        playerScreenPosition = Main.WorldToViewportPoint(Player.position);
        Debug.Log(playerScreenPosition);
        var screendistance = new Vector3(0.5f, 0.5f, 0) - playerScreenPosition;
        if (Mathf.Abs(screendistance.x) > .125f || Mathf.Abs(screendistance.y) > .25f)
        {
            transform.forward = Vector3.Lerp(transform.forward, Player.forward, Time.deltaTime);

        }

        var targetsVect = Target.position - Player.position;
        midepoint = Player.position + 0.5f * targetsVect + Vector3.up;
        Main.transform.LookAt(midepoint);

        distBetTargs = targetsVect.magnitude;
        cameraDistance = (distBetTargs / 2.0f / aspectRatio) / tanFov;
        Player.forward = targetsVect;
        Vector3 dir = (Main.transform.position - midepoint).normalized;
        Main.transform.position = midepoint + dir * (cameraDistance + DISTANCE_MARGIN);

    }

    private void LateUpdate()
    {
        transform.position = Player.position;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(midepoint, 1);
        Gizmos.DrawRay(Player.position, Player.forward);
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}

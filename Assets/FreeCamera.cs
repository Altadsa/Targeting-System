using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public Camera Main;
    public Transform Player;
    public float MaxDistance = 3f;
    private bool move = false;
    public float moveTime = 1;
    public float startTime = 0;

    private float DistanceToPlayer => Vector3.Distance(transform.position, Player.position);
    private Vector3 CameraPosition => Player.position - Player.forward * MaxDistance;

    private void Update()
    {
        if (DistanceToPlayer > MaxDistance && !move)
        {
            move = true;
            startTime = Time.time;
        }
    }

    private void LateUpdate()
    {
        Main.transform.LookAt(Player);
        var dir = Player.position - transform.position;
        transform.forward = dir.normalized;
        if (move)
        {
            var delta = Time.time - startTime;
            delta /= moveTime;
            if (delta > 1)
            {
                delta = 1;
                move = false;
            }

            transform.position = Vector3.Lerp(transform.position, CameraPosition, delta);
        }
    }

}
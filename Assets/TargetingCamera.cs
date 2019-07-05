using System.Linq;
using UnityEngine;

public class TargetingCamera : MonoBehaviour
{
    // Player and Target transforms
    public Transform Player;
    public Transform Target;
    public Vector3 Offset;
    public float SpeedOffset = 5;

    public LayerMask LayerMask;

    //The angle between the midpoint of the Targets and the Camera
    private const float OFFSET = 30;

    // Directional Vector between the Player and Target
    private Vector3 _midpointVector;
    //World Coordinates of the midpoint between Player and Target
    private Vector3 _targetMidpoint;
    //Distance behind the Player that the Camera should stay.
    public float TargetingDistance;
    //Boolean to determine whether Camera should orient itself to Player on left or right
    private bool _directionToRotate;

    private Vector3 _fixedForward;

    //Enable Player ability to move if coming from First Person
    private void OnEnable()
    {
        Target = GetTarget();
        if (!Target)
            _fixedForward = transform.forward;
    }


    private void Update()
    {
        if (Target)
        {
            _midpointVector = Target.position - Player.position;
            _targetMidpoint = Player.position + 0.5f * _midpointVector;
            var camMidAngle = Vector3.SignedAngle((_midpointVector - transform.position).normalized, _midpointVector.normalized, Vector3.up);
            _directionToRotate = camMidAngle >= Mathf.Epsilon;

        }

    }

    private void LateUpdate()
    {
        if (Target)
        {
            var newPosition = TargetingPosition();
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * SpeedOffset);
            transform.forward = (_targetMidpoint - transform.position).normalized;
        }
        else
        {
            transform.position = Player.position - _fixedForward * TargetingDistance + Offset;
            transform.forward = _fixedForward;
        }

    }

    private Vector3 TargetingPosition()
    {
        var rot = !_directionToRotate ? OFFSET : -OFFSET;
        var rotFor = Quaternion.AngleAxis(rot, Vector3.up) * -_midpointVector.normalized;
        return Player.position + rotFor * TargetingDistance + Offset;
    }

    private Transform GetTarget()
    {
        var targets = Physics.OverlapBox(Player.position, new Vector3(20, 20, 20), Quaternion.identity, LayerMask).ToList();
        if (targets.Count == 0) return null;
        var frontTargets = targets.Where(t =>
            Vector3.Dot(Player.forward,
                (t.transform.position - Player.position).normalized) > 0);
        return frontTargets.OrderBy(t => Vector3.Distance(Player.position, t.transform.position)).First().transform;
    }
}

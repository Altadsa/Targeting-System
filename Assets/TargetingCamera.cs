using System.Collections;
using System.Linq;
using UnityEngine;

public class TargetingCamera : PlayerCamera
{
    [Header ("Targets")]
    // Player and Target transforms
    [SerializeField] Transform Player;
    public Transform Target;

    [Header("Variables")]
    public Vector3 TargetingArea;
    public float SpeedOffset = 5;

    [Tooltip("The Targetable GameObjects Layer")]
    public LayerMask LayerMask;

    //Minimum Distance behind the Player that the Camera should stay.
    private const float MIN_TARGETING_DISTANCE = 3f;

    float DistanceOffset => _midpointVector.magnitude < 5 ? 0 : MAX_POS_OFFSET * _midpointVector.magnitude / 10;

    private const float MAX_POS_OFFSET = 2;
    //The angle between the midpoint of the Targets and the Camera
    private const float ANGLE_OFFSET = 45;

    // Directional Vector between the Player and Target
    private Vector3 _midpointVector;
    //World Coordinates of the midpoint between Player and Target
    private Vector3 _targetMidpoint;

    //Boolean to determine whether Camera should orient itself to Player on left or right
    private bool _directionToRotate;

    private Vector3 _fixedForward;

    //Gets the Fixed position behind the Player
    private Vector3 FixedFollow => Player.position - _fixedForward * MIN_TARGETING_DISTANCE * 2 + Vector3.up;

    //Enable Player ability to move if coming from First Person
    private void OnEnable()
    {
        Target = GetTarget();
        if (!Target)
        {
            _fixedForward = Player.forward;
            StartCoroutine(MoveToFixedForward());
        }
    }


    private void Update()
    {
        if (Target)
        {
            Debug.Log(_midpointVector.magnitude);
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
            var newPosition = CheckForCollision(_targetMidpoint, TargetingPosition());
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * SpeedOffset);
            transform.LookAt(_targetMidpoint);
        }
        else
        {
            if (_inPosition)
                transform.position = CheckForCollision(Player.position, FixedFollow);
        }
    }


    IEnumerator MoveToFixedForward()
    {
        _inPosition = false;
        var newPos = CheckForCollision(Player.position, FixedFollow);
        var newRot = Quaternion.LookRotation(Player.position - newPos);
        var startTime = Time.time;
        while (Time.time - startTime < MOVE_DURATION)
        {
            var delta = Time.time - startTime;
            if (delta > 1)
            {
                delta = 1;
            }
            transform.position = Vector3.Lerp(transform.position, newPos, delta);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, delta);
            yield return new WaitForEndOfFrame();
        }

        _inPosition = true;
    }

    private Vector3 TargetingPosition()
    {
        var rot = !_directionToRotate ? ANGLE_OFFSET : -ANGLE_OFFSET;
        var rotFor = Quaternion.AngleAxis(rot, Vector3.up) * -Vector3.Scale(_midpointVector, new Vector3(1,0,1)).normalized;
        var offset = DistanceOffset;
        var heightOffset = new Vector3(0, 1 + offset, 0);
        var distanceOffset = MIN_TARGETING_DISTANCE + offset;
        return Player.position + rotFor * distanceOffset + heightOffset;
    }

    private Transform GetTarget()
    {
        var targets = Physics.OverlapBox(Player.position + Player.forward * TargetingArea.z/2, TargetingArea, Quaternion.identity, LayerMask).ToList();
        if (targets.Count == 0) return null;

        var frontTargets = targets.Where(IsTargetInCameraView);
        return frontTargets.OrderBy(t => Vector3.Distance(Player.position, t.transform.position)).FirstOrDefault()?.transform;
    }

    private bool IsTargetInCameraView(Collider targetCollider)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
        var inPlane = GeometryUtility.TestPlanesAABB(planes, targetCollider.bounds);
        var infront = Vector3.Dot(Player.forward,
                          (targetCollider.transform.position - Player.position).normalized) > 0;
        return infront && inPlane;
    }
}

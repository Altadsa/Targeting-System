using System.Collections;
using UnityEngine;

public abstract class PlayerCamera : MonoBehaviour
{
    float _startMove = 0;
    float _moveDuration = 0.5f;

    protected bool _inPosition = false;
    protected RaycastHit _hit;
    protected static LayerMask _layerMask;

    private void Awake()
    {
        _layerMask = LayerMask.GetMask("Player", "Targetable");
    }

    protected Vector3 CheckForCollision(Vector3 sourcePosition, Vector3 cameraPosition)
    {
        var direction = cameraPosition - sourcePosition;
        bool colliding = Physics.Raycast(sourcePosition, direction.normalized, out _hit, direction.magnitude, ~(_layerMask));
        return colliding ? _hit.point : cameraPosition;
    }

    protected IEnumerator MoveToPosition(Vector3 newPosition)
    {
        _inPosition = false;
        _startMove = Time.time;
        while (Time.time - _startMove < _moveDuration)
        {
            var delta = Time.time - _startMove;
            delta /= _moveDuration;
            if (delta > 1)
            {
                delta = 1;
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, delta);
            yield return new WaitForEndOfFrame();
        }
        _inPosition = true;
    }

}
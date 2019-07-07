using System.Collections;
using UnityEngine;

public abstract class PlayerCamera : MonoBehaviour
{
    float _startMove = 0;
    float _moveDuration = 1;

    protected bool _inPosition = false;

    protected IEnumerator MoveToPosition(Transform newTransform)
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

            transform.rotation = Quaternion.Slerp(transform.rotation, newTransform.rotation, delta);
            transform.position = Vector3.Lerp(transform.position, newTransform.position, delta);
            yield return new WaitForEndOfFrame();
        }
        _inPosition = true;
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
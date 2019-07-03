using System.Collections;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    //Transform of the Player
    public Transform Player;
    //Free Camera Offset
    public Vector3 Offset;
    //Distance behind the Player to move to
    public float MaxDistance = 3f;

    //Property to get the desired position behind the Player
    private Vector3 CameraPosition => Player.position - Player.forward * MaxDistance + Offset;

    private bool _isInitialized = false;

    private void OnEnable()
    {
        StartCoroutine(MoveToView());
    }

    float sT = 0;
    float duration = 1;
    IEnumerator MoveToView()
    {
        sT = Time.time;
        while (Time.time - sT < duration)
        {
            var delta = Time.time - sT;
            delta /= duration;
            if (delta > 1)
            {
                delta = 1;
            }
            transform.position = Vector3.Lerp(transform.position, CameraPosition, delta);
            yield return new WaitForEndOfFrame();
        }
        _isInitialized = true;
    }


    private void LateUpdate()
    {
        transform.LookAt(Player);
        if (_isInitialized)
        {
            var distance = Vector3.Distance(transform.position, CameraPosition);
            transform.position = Vector3.Lerp(transform.position, CameraPosition, Time.deltaTime * distance);
        }
    }

}
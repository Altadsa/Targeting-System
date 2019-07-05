using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Hookshot : MonoBehaviour
{

    public float Distance = 10;
    public Transform Hook;
    LineRenderer _lineRenderer;
    private bool _launched;
    private Camera _main;
    private Vector3 _destination;


    private float _shotduration = 2;
    private float _shotStart = 0;


    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPositions(new Vector3[] { transform.position, Hook.position });
        _main = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && !_launched)
        {
            _launched = true;
            var camWorldPoint = _main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Distance));
            _destination = camWorldPoint;
            _shotStart = Time.time;
            Hook.forward = (_destination - transform.position).normalized;
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, transform.position);
        }

        if (_launched)
        {
            var delta = Time.time - _shotStart;
            delta /= _shotduration;
            if (delta > 1)
            {
                delta = 1;
                _launched = false;
            }
            var distanceToDestination = Vector3.Distance(Hook.position, _destination);
            Hook.position = Vector3.Lerp(Hook.position, _destination, delta / distanceToDestination);
            _lineRenderer.SetPosition(1, Hook.position);
        }
        else
        {
            Hook.position = transform.position;
            _lineRenderer.enabled = false;
        }
        
    }

    public void HookToPosition(Vector3 position)
    {
        _launched = false;
        StartCoroutine(PullPlayer(position));
    }

    IEnumerator PullPlayer(Vector3 position)
    {
        var player = FindObjectOfType<PlayerController>().GetComponent<Rigidbody>();
        var dir = position - player.transform.position;
        var newPosition = position - dir.normalized * 2.5f;
        var startTime = Time.time;
        var duration = 1.5f;
        while (Time.time - startTime < duration)
        {
            var delta = Time.time - startTime;
            delta /= duration;
            if (delta > 1)
            {
                delta = 1;
            }

            var distanceToPosition = Vector3.Distance(player.transform.position, newPosition);

            player.position = Vector3.Lerp(player.transform.position, newPosition, delta / distanceToPosition);
            yield return new WaitForEndOfFrame();
        }
    }

}

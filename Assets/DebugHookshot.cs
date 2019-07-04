using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DebugHookshot : MonoBehaviour
{

    public float Distance = 10;
    public Transform Hook;
    LineRenderer _lineRenderer;
    private bool launched;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && !launched)
        {
            launched = true;
            _destination = _main.transform.position + _main.transform.forward * Distance;
            _shotStart = Time.time;
            Hook.forward = (_destination - transform.position).normalized;
            _lineRenderer.enabled = true;
        }

        if (launched)
        {
            var delta = Time.time - _shotStart;
            delta /= _shotduration;
            if (delta > 1)
            {
                delta = 1;
                launched = false;
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


}

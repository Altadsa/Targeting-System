using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugAi : MonoBehaviour
{
    private NavMeshAgent _agent;

    private Vector3 RandomDestination => transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        while (!_agent.hasPath)
        {
            NavMeshPath path = new NavMeshPath();
            if (_agent.CalculatePath(RandomDestination, path))
            {
                _agent.SetPath(path);
            }
        }
    }
}

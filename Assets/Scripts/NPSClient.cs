using NTC.MonoCache;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPSClient : MonoCache
{
    public List<Transform> stolPoints;
    private NavMeshAgent agent;
    private Transform currentTarget;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = stolPoints[Random.Range(0, stolPoints.Count)].transform;
    }

    protected override void Run()
    {
        agent.SetDestination(currentTarget.position);
        if (agent.remainingDistance < 0.1f)
        {
            currentTarget = stolPoints[Random.Range(0, stolPoints.Count)].transform;
        }
    }
}

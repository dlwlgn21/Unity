using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    public Transform[] wayPoints;
    int mCurWayIdx;
    void Start()
    {
        navMeshAgent.SetDestination(wayPoints[0].position);
    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            mCurWayIdx = (mCurWayIdx + 1) % wayPoints.Length;
            navMeshAgent.SetDestination(wayPoints[mCurWayIdx].position);

        }
    }
}

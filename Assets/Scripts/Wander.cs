using System.Collections.Generic;
using UnityEngine;

public class Wander : State
{
    private Transform _currentWaypoint;
    
    public Wander(FSMAI fsmai) : base(fsmai)
    {
    }

    public override void Enter()
    {
        _currentWaypoint = GetRandomWaypoint();
    }

    public override void Execute()
    {
        fsmai.Agent.SetDestination(_currentWaypoint.position);
        if (Vector3.Distance(_currentWaypoint.position, fsmai.transform.position) < fsmai.PointDistanceThreshold) 
            _currentWaypoint = GetRandomWaypoint();
    }

    public override void Exit()
    {
    }
    
    public Transform GetRandomWaypoint()
    {
        return fsmai.Waypoints[Random.Range(0, fsmai.Waypoints.Count)];
    }
}
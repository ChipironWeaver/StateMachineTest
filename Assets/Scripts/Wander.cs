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
        _currentWaypoint = fsmai.waypoints[Random.Range(0, fsmai.waypoints.Count)];
        Debug.Log(_currentWaypoint.gameObject.name);
    }

    public override void Execute()
    {
        fsmai.Agent.SetDestination(_currentWaypoint.position);
        float distance = Vector3.Distance(_currentWaypoint.position, fsmai.transform.position);
        if (distance < 0.5f)
        {
            Debug.Log("change");
            _currentWaypoint = fsmai.waypoints[Random.Range(0, fsmai.waypoints.Count)];
        }
    }

    public override void Exit()
    {
    }
}
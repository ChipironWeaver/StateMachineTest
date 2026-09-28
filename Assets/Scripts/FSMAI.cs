using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class FSMAI : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private StateEnum _defaultState;
    
    
    [Header("Wander")]
    [SerializeField] private float _pointDistanceThreshold;
    
    [Header("Chase")] 
    [SerializeField] private TagDetectionTrigger _chaseColliderDetection;
    
    public NavMeshAgent Agent => _agent;
    public List<Transform> Waypoints => _waypoints;
    public float PointDistanceThreshold => _pointDistanceThreshold;
    public TagDetectionTrigger ChaseColliderDetection => _chaseColliderDetection;
    
    private State _currentState;
    
    private void Start()
    {
        _currentState = GetState(_defaultState);
    }

    private void Update()
    {
        _currentState.Execute();
        if (_currentState is Wander && _chaseColliderDetection.IsAnyoneDetected())
        {
            SwitchState(new Chase(this));
        }
        if (_currentState is Chase && !_chaseColliderDetection.IsAnyoneDetected())
        {
            SwitchState(new Wander(this));
        }
    }
    
    
    private void SwitchState(State newState)
    {
        print("switching to " + newState);
        _currentState?.Exit();
        _currentState = newState;
    }

    private State GetState(StateEnum state)
    {
        switch (state)
        {
            case StateEnum.Wander:
                return new Wander(this);
            case StateEnum.Chase:
                return new Chase(this);
            default:
                Debug.LogWarning("State " + state.ToString() + " not implemented");
                return null;
        }
    }
    
    private enum StateEnum
    {
        Wander,
        Chase
    }

    
}
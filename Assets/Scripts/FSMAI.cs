using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class FSMAI : MonoBehaviour
{
    public List<Transform> waypoints;
    [SerializeField] NavMeshAgent _agent;
    
    public NavMeshAgent Agent => _agent;
    
    
    private State _currentState;
    
    private void Start()
    {
        _currentState = new Wander(this);
    }

    private void Update()
    {
        _currentState.Execute();
    }
}
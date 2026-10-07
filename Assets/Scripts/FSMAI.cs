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
    
    [Header("Attack")]
    [SerializeField] private TagDetectionTrigger _attackColliderDetection;
    [SerializeField] private Rigidbody _rigidbody;
    
    [SerializeField] private float _damage;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _rangedAttackThreshold;

    [SerializeField] private GameObject _rangeAttackPrefab;
    
    public NavMeshAgent Agent => _agent;
    public List<Transform> Waypoints => _waypoints;
    public float PointDistanceThreshold => _pointDistanceThreshold;
    public TagDetectionTrigger ChaseColliderDetection => _chaseColliderDetection;
    public float Damage => _damage;
    public float RangedAttackThreshold => _rangedAttackThreshold;
    public float AttackCooldown => _attackCooldown;
    
    
    
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
        if (_currentState is Attack && !_chaseColliderDetection.IsAnyoneDetected())
        {
            SwitchState(new Wander(this));
        }
        else if (_currentState is Chase && _attackColliderDetection.IsAnyoneDetected())
        {
            SwitchState(new Attack(this));
        }
        else if (_currentState is Chase && !_chaseColliderDetection.IsAnyoneDetected())
        {
            SwitchState(new Wander(this));
        }
    }

    public void LaunchRangeAttack()
    {
        Vector3 direction = _agent.destination - transform.position;
        direction.Normalize();
        direction *= 500;
        GameObject projectile = Instantiate(_rangeAttackPrefab);
        projectile.transform.position = transform.position;
        projectile.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
    }

    public void LaunchAttack()
    {
        Debug.Log("im attacking close");
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
            case StateEnum.Attack:
                return new Attack(this);
            default:
                Debug.LogWarning("State " + state.ToString() + " not implemented");
                return null;
        }
    }
    
    private enum StateEnum
    {
        Wander,
        Chase,
        Attack
    }

    
}
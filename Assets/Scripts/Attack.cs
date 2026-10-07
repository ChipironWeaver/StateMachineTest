using UnityEngine;

public class Attack : State
{
    public Attack(FSMAI fsmai) : base(fsmai)
    {
    }

    private Transform _target;
    private float _attackChargingTime;
    
    public override void Enter()
    {
        _target = fsmai.ChaseColliderDetection.GetNeerestCollider(fsmai.transform.position).transform;
        fsmai.Agent.SetDestination(fsmai.transform.position);
    }

    public override void Execute()
    {
        if (!_target) return;
        _attackChargingTime +=  Time.deltaTime;
        if (_attackChargingTime > fsmai.AttackCooldown)
        {
            LaunchAttack();
            _attackChargingTime = 0;
        }
        
        
    }

    public void LaunchAttack()
    {
        float distance = Vector3.Distance(_target.position, fsmai.transform.position);
        if (distance > fsmai.RangedAttackThreshold)
        {
            fsmai.LaunchRangeAttack();
        }
        else
        {
            fsmai.LaunchAttack();
        }
    }

    public override void Exit()
    {
        
    }
}
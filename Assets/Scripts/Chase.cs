using UnityEngine;

public class Chase : State
{
    public Chase(FSMAI fsmai) : base(fsmai)
    {
    }

    private Transform _target;
    public override void Enter()
    {
        if (fsmai.ChaseColliderDetection.IsAnyoneDetected())
        {
            _target = fsmai.ChaseColliderDetection.GetNeerestCollider(fsmai.transform.position).transform;
        }
    }

    public override void Execute()
    {
        fsmai.Agent.SetDestination(_target.position);
    }

    public override void Exit()
    {
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ooze Idle State", menuName = "Scriptables/AI States/Ooze Idle State")]
public class IdleOozeState : AbstractState
{
    public AbstractState chaseState;

    public float wanderRadius;
    public float wanderForwardOffset;
    public float wanderStrength;
    float angle = 0;
    public override void EnterState(Entity e)
    {
        base.EnterState(e);
        e.anim.Play("IDLE");
    }
    public override void UpdateState(Entity e)
    {
        e.agent.SetDestination(((OozeEntity)e).home + Navigation.instance.wander(e.transform, wanderRadius, wanderForwardOffset, wanderStrength, ref angle));

        if(Navigation.instance.GetDistanceToPlayer(e.transform.position) < ((OozeEntity)e).chaseRadius) 
        {
            e.fsm.ChangeState(chaseState);
        }
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

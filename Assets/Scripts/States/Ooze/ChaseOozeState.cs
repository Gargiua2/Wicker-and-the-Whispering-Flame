using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ooze Chase State", menuName = "Scriptables/AI States/Ooze Chase State")]
public class ChaseOozeState : AbstractState
{
    public AbstractState attackState;
    public AbstractState idleState;

    public override void EnterState(Entity e)
    {
        base.EnterState(e);
        e.anim.Play("IDLE");
    }

    public override void UpdateState(Entity e)
    {
        e.agent.SetDestination(Navigation.instance.FindNearestPointOnRadiusWithPlayerLOS(2.5f, e.transform.position));

        float d = Navigation.instance.GetDistanceToPlayer(e.transform.position);

        if (d < ((OozeEntity)e).attackRadius) 
        {
            e.fsm.ChangeState(attackState);
        } else if (d > ((OozeEntity)e).leashRadius) 
        {
            e.fsm.ChangeState(idleState);
        }
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wraith Target State", menuName = "Scriptables/AI States/Wraith Target State")]
public class WraithTargetState : AbstractState
{
    public AbstractState idleState;
    public AbstractState attackState;
    WraithEntity WEntity;

    public override void EnterState(Entity e)
    {

        WEntity = e as WraithEntity;

        e.anim.Play("IDLE");
        e.agent.SetDestination(Navigation.instance.FindNearestPointOnRadiusWithPlayerLOS(WEntity.targetRadius, e.transform.position));
    }

    public override void UpdateState(Entity e)
    {
        if (Navigation.instance.GetDistanceToPlayer(e.transform.position) < WEntity.safetyRadius) 
        {
            e.agent.speed = 14f;
        } else 
        {
            e.agent.speed = 3.5f;
        }


        if(Navigation.instance.GetDistanceToPlayer(e.transform.position) < WEntity.targetRadius + 1 && Navigation.instance.GetDistanceToPlayer(e.transform.position) > WEntity.safetyRadius ) 
        {
            e.fsm.ChangeState(attackState);
        }
        else if (Navigation.instance.GetDistanceToPlayer(e.transform.position) > WEntity.leashRadius) 
        {
            e.fsm.ChangeState(idleState);
        } else 
        {
            e.agent.SetDestination(Navigation.instance.FindNearestPointOnRadiusWithPlayerLOS(WEntity.targetRadius, e.transform.position));
        }
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}
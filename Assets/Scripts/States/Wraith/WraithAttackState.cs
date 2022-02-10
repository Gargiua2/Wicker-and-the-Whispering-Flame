using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wraith Attack State", menuName = "Scriptables/AI States/Wraith Attack State")]
public class WraithAttackState : AbstractState
{
    public AbstractState targetState;
    WraithEntity EWraith;


    public override void EnterState(Entity e)
    {
        Debug.Log("ENTERING STATE " + this.GetType().ToString());

        EWraith = e as WraithEntity;
        e.anim.Play("IDLE");
        e.agent.SetDestination(e.transform.position);
    }

    public override void UpdateState(Entity e)
    {

        if (!EWraith.attackCooldown)
        {
            e.Attack();
        }

        if(Navigation.instance.GetDistanceToPlayer(e.transform.position) > EWraith.attackRadius || Navigation.instance.GetDistanceToPlayer(e.transform.position) < EWraith.dangerRadius)
        {
            e.fsm.ChangeState(targetState);
        }
    }


    public override void ExitState(Entity e)
    {
        EWraith.EndAttack();
        base.ExitState(e);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ooze Attack State", menuName = "Scriptables/AI States/Ooze Attack State")]
public class AttackOozeState : AbstractState
{
    public AbstractState chaseState;
    public float attackDelay = 1f;
    bool attackCooldown = false;
    public override void EnterState(Entity e)
    {
        base.EnterState(e);
        e.anim.Play("ATTACK");
        e.agent.SetDestination(e.transform.position);
    }

    public override void UpdateState(Entity e)
    {
        float d = Navigation.instance.GetDistanceToPlayer(e.transform.position);

        if (d > ((OozeEntity)e).attackRange && !attackCooldown)
        {
            e.fsm.ChangeState(chaseState);
            return;
        }

        if (!attackCooldown)
        {
            e.Attack();
            attackCooldown = true;
            e.StartCoroutine(e.GenericTimer(attackDelay, this, EndCooldown));
        }
    }

    public void EndCooldown() 
    {
        attackCooldown = false;
    }


    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

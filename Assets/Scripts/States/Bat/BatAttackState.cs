using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Attack State", menuName = "Scriptables/AI States/Bat Attack State")]
public class BatAttackState : AbstractState
{
    [SerializeField] float cooldownTime;
    [SerializeField] float leashRange = 15f;
    [SerializeField] AbstractState chaseState;
    [SerializeField] AbstractState returnState;
    
    bool cooldown = false;
    BatEntity bat;

    public override void EnterState(Entity e)
    {
        bat = e as BatEntity;

        base.EnterState(e);
    }

    public override void UpdateState(Entity e)
    {
        if (!cooldown) 
        {
            if(Vector3.Distance(e.transform.position, Player.instance.transform.position) < bat.meleeRange) 
            {
                e.Attack();
                cooldown = true;
                e.StartCoroutine(e.GenericTimer(cooldownTime, this, attackCooldown));
            }
            else 
            {
                e.fsm.ChangeState(chaseState);
                return;
            }
            
        }

        if (Vector3.Distance(e.transform.position, Player.instance.transform.position) > leashRange) 
        {
        
        }

    }

    public void attackCooldown() 
    {
        cooldown = false;
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

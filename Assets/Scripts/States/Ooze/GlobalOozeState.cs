using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ooze Global State", menuName = "Scriptables/AI States/Ooze Global State")]
public class GlobalOozeState : AbstractState
{
    public AbstractState deadState;
    public AbstractState damagedState;

    float pHP;
    public override void EnterState(Entity e)
    {
        
        base.EnterState(e);
        pHP = e.HP;
    }
    public override void UpdateState(Entity e)
    {
        if (!e.alive)
            return;

        if (e.HP <= 0) 
        {
            e.Die();
            e.fsm.ChangeState(deadState);
        }

        if(e.HP < pHP && ((OozeEntity)e).curInDamageState == false) 
        {
            pHP = e.HP;
            e.fsm.ChangeState(damagedState);
        }

    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

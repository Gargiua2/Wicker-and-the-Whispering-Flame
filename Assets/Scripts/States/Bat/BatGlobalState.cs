using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Global State", menuName = "Scriptables/AI States/Bat Global State")]
public class BatGlobalState : AbstractState
{
    [SerializeField] AbstractState damageState;
    [SerializeField] AbstractState deadState;
    float prevHP;

    public override void EnterState(Entity e)
    {
        prevHP = e.HP;

        base.EnterState(e);
    }

    public override void UpdateState(Entity e)
    {
        if (!e.alive)
            return;

        if (e.HP <= 0) 
        {
            e.fsm.ChangeState(deadState);
        }
        else if (prevHP > e.HP) 
        {
            prevHP = e.HP;
            e.fsm.ChangeState(damageState);
        }
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wraith Global State", menuName = "Scriptables/AI States/Wraith Global State")]
public class WraithGlobalState : AbstractState
{
    public AbstractState damageState;
    public AbstractState deadState;
    float pHP;

    float baseH;
    float heightOffset;
    float v;

    public override void EnterState(Entity e)
    {

       
        pHP = e.HP;
    }

    public override void UpdateState(Entity e)
    {

        if (!e.alive)
            return;

        heightOffset += Mathf.Sin(5 * Time.time) / 100 * Time.deltaTime * 60;
        baseH = Mathf.SmoothDamp(baseH, Player.instance.transform.position.y + .2f, ref v, .25f);
        e.agent.baseOffset = baseH + heightOffset;

        if (e.HP <= 0) 
        {
            e.fsm.ChangeState(deadState);
            e.Die();
        }
        else if(e.HP < pHP) 
        {
            pHP = e.HP;
            e.fsm.ChangeState(damageState);
        }
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "Ooze Dead State", menuName = "Scriptables/AI States/Ooze Dead State")]
public class DeadOozeState : AbstractState
{

    public override void EnterState(Entity e)
    {
        base.EnterState(e);
        e.anim.Play("DAMAGED");
        ((OozeEntity)e).curInDamageState = true;
        e.transform.DOScale(0, ((OozeEntity)e).oozeDeathRate);
        e.agent.isStopped = true;
    }

    public override void UpdateState(Entity e)
    {
        
    }

    public override void ExitState(Entity e)
    {
        ((OozeEntity)e).curInDamageState = false;
        base.ExitState(e);
    }
}

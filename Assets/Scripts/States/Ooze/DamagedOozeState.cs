using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ooze Damage State", menuName = "Scriptables/AI States/Ooze Damage State")]
public class DamagedOozeState : AbstractState
{
    public float damageLength;
    Entity e_;
    public override void EnterState(Entity e)
    {
        
        base.EnterState(e);
        e_ = e;
        e.anim.Play("DAMAGED");
        e.agent.SetDestination(e.transform.position);
    }
    public override void UpdateState(Entity e)
    {
        e.StartCoroutine(e.GenericTimer(damageLength, this, Blip));
    }

    public void Blip() 
    {
        Debug.Log("OOZE DAMAGE REVERT CALLED");
        e_.fsm.RevertToPreviousState();
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

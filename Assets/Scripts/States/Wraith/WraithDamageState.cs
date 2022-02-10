using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wraith Damage State", menuName = "Scriptables/AI States/Wraith Damage State")]
public class WraithDamageState : AbstractState
{
    public float stunLength;

    Entity _e;

    public override void EnterState(Entity e)
    {
        Debug.Log("ENTERING STATE " + this.GetType().ToString());

        _e = e;

        e.StartCoroutine(e.GenericTimer(stunLength, this, EndDamageStun));

        e.anim.Play("DAMAGE");
        e.agent.isStopped = true;
        e.invuln = true;
    }

    public override void UpdateState(Entity e)
    {

    }

    public void EndDamageStun() 
    {
        _e.fsm.RevertToPreviousState();
    }

    public override void ExitState(Entity e)
    {
        e.agent.isStopped = false;
        e.invuln = false;

        base.ExitState(e);
    }
}

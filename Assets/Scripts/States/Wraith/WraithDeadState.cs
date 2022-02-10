using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wraith Dead State", menuName = "Scriptables/AI States/Wraith Dead State")]
public class WraithDeadState : AbstractState
{
    public override void EnterState(Entity e)
    {
        Debug.Log("ENTERING STATE " + this.GetType().ToString());

        e.agent.enabled = false;
        e.anim.Play("DEAD");
    }

    public override void UpdateState(Entity e)
    {
        e.anim.Play("DEAD");
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

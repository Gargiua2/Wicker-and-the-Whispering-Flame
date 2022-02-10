using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Dead State", menuName = "Scriptables/AI States/Bat Dead State")]
public class BatDeadState : AbstractState
{
    public override void EnterState(Entity e)
    {
        e.agent.enabled = false;

        e.anim.Play("DAMAGE");

        e.Die();

        base.EnterState(e);
    }

    public override void UpdateState(Entity e)
    {

    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

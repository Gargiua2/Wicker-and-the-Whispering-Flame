using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Return State", menuName = "Scriptables/AI States/Bat Return State")]
public class BatReturnState : AbstractState
{
    [SerializeField] AbstractState idleState;
    BatEntity bat;
    float v;
    public override void EnterState(Entity e)
    {
        e.anim.Play("FLIGHT");
        bat = e as BatEntity;
        base.EnterState(e);
    }

    public override void UpdateState(Entity e)
    {
        e.agent.baseOffset = Mathf.SmoothDamp(e.agent.baseOffset, bat.home.y - .2f, ref v, .25f);

        e.agent.SetDestination(bat.home);

        if (Vector3.Distance(e.transform.position, bat.home) < .5f) 
        {
            e.fsm.ChangeState(idleState);
        }
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

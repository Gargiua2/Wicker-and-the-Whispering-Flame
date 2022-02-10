using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Damage State", menuName = "Scriptables/AI States/Bat Damage State")]
public class BatDamageState : AbstractState
{
    [SerializeField] float stunLength = .5f;
    bool stun = true;
    public override void EnterState(Entity e)
    {
        e.GetComponent<Animator>().Play("DAMAGE");

        if(e.agent.enabled == true)
        e.agent.SetDestination(e.transform.position);

        e.StartCoroutine(e.GenericTimer(stunLength, this, endDamage));

        base.EnterState(e);
    }

    public void endDamage() 
    {
        Debug.Log("Damage Timer Ended");
        stun = false;
    }

    public override void UpdateState(Entity e)
    {
        if (!stun)
            e.fsm.RevertToPreviousState();
    }

    public override void ExitState(Entity e)
    {

        base.ExitState(e);
    }
}

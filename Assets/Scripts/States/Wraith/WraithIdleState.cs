using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wraith Idle State", menuName = "Scriptables/AI States/Wraith Idle State")]
public class WraithIdleState : AbstractState
{

    public AbstractState targetState;
    public float wanderRadius;
    
    WraithEntity WEntity;
    


    public override void EnterState(Entity e)
    {


        WEntity = e as WraithEntity;
        e.anim.Play("IDLE");
        StartNewWander();
    }

    public override void UpdateState(Entity e)
    {
        

        if (e.pathComplete()) 
        {
            StartNewWander();
        }

        if (Navigation.instance.GetDistanceToPlayer(e.transform.position) < WEntity.leashRadius) 
        {
            e.fsm.ChangeState(targetState);
        }

    }

    public void StartNewWander() 
    {
        Vector2 wanderVector = Random.insideUnitCircle.normalized * Random.Range(-wanderRadius, wanderRadius);

        Vector3 wanderPoint = new Vector3(wanderVector.x, 0, wanderVector.y) + WEntity.homePosition;

        WEntity.agent.SetDestination(wanderPoint);
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

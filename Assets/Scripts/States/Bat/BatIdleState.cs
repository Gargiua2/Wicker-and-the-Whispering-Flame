using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Idle State", menuName = "Scriptables/AI States/Bat Idle State")]
public class BatIdleState : AbstractState
{
    [SerializeField] Sprite batIdleSprite;
    [SerializeField] AbstractState batChase;
    [SerializeField] float BatAggroRadius;

    
    public override void EnterState(Entity e)
    {
        e.agent.enabled = false;

        BatEntity bat = e as BatEntity;
        e.transform.position = bat.home;
        e.gameObject.GetComponent<ImposterSprite>().SetImposterSprite(new Sprite[1] { batIdleSprite }, ImposterSpriteDrawMode.FLAT, false, 180);
        e.GetComponent<Animator>().Play("IDLE");

       
        base.EnterState(e);

    }

    public override void UpdateState(Entity e)
    {

        if (Navigation.instance.GetDistanceToPlayer(e.transform.position) < BatAggroRadius || e.HP < ((BatEntity)e).pHP) 
        {
            ((BatEntity)e).pHP = e.HP;

            e.fsm.ChangeState(batChase);
        }
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

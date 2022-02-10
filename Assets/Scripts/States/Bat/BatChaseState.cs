using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State bat entities use when chasing the player.

//Create an asset menu for this state. Allows us to drag-and-drop the state in the inspector, 
//as well as set values in the inspector.
[CreateAssetMenu(fileName = "Bat Chase State", menuName = "Scriptables/AI States/Bat Chase State")]
public class BatChaseState : AbstractState
{
    //Settings/Parameters.
    [SerializeField] Sprite batSprite; //Sprite to use while in this state.
    [SerializeField] float leashRange = 25f; //The distance in which the bat will continue to chase the player.
    [SerializeField] AbstractState attackState; //Reference to the bat's attack state.
    [SerializeField] AbstractState returnState; //Reference to the bat's return state.

    //Local Variables/References.
    BatEntity bat;
    float baseH;
    float heightOffset;
    float v;


    public override void EnterState(Entity e)
    {
        //Set the sprite when entering this state.
        e.GetComponent<ImposterSprite>().SetImposterSprite(new Sprite[1] { batSprite }, ImposterSpriteDrawMode.FLAT, true, 0);

        //Play our bat flight animation.
        e.GetComponent<Animator>().Play("FLIGHT");

        //Set our NavMeshAgent to enabled, set our offset form the ground.
        e.agent.enabled = true;
        baseH = e.agent.baseOffset = e.transform.position.y - .5f;

        //Cast our entity to a bat, save a reference for future use.
        bat = e as BatEntity;
        base.EnterState(e);
    }
    
    public override void UpdateState(Entity e)
    {
        //Handles the up-and-down flying animation the bat should use when flying around.
        heightOffset += Mathf.Sin(5*Time.time) / 100 * Time.deltaTime * 60;
        baseH = Mathf.SmoothDamp(baseH, Player.instance.transform.position.y + .5f, ref v, .25f);
        e.agent.baseOffset = baseH + heightOffset;

        //Set the destination of the bat to a point near the player where we can properly attack them.
        //Uses our Naviagtion helper functions to find this point.
        e.agent.SetDestination(Navigation.instance.FindPointOnRadius(Player.instance.transform.position, e.transform.position, bat.meleeRange - 1.5f));

        //If we are near enough to the player to attack, change to the attack state.
        if (Vector3.Distance(e.transform.position, Player.instance.transform.position) < bat.meleeRange - 1) 
        {
            e.fsm.ChangeState(attackState);
            return;
        }

        //If the player is too far away (or we use a debug key) change to the return state.
        if(Vector3.Distance(e.transform.position, Player.instance.transform.position) > leashRange || Input.GetKeyDown(KeyCode.Z)) 
        {
            e.fsm.ChangeState(returnState);
            return;
        }
    }

    public override void ExitState(Entity e)
    {
        base.ExitState(e);
    }
}

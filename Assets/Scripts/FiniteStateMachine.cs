using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Impimentation of a finite state machine. 
//All Entities have an FSM, which they use to transition between different behaviour states.
//This class handles the trnasitions between those states, and updates those states as necessary.
public class FiniteStateMachine : MonoBehaviour
{
    [SerializeField]AbstractState startingState; //The state we want to load when this entity spawns.
    [SerializeField] AbstractState globalState; //This state is updated every frame regardless of the current state.
    AbstractState currentState; //The active state.

    AbstractState previousState; //Stores a reference to the previous state we were in.

    Entity e; //A reference to the entity which this FSM is attatched to.

    void Awake()
    {
        //Grab the entity this FSM is attached to.
        e = GetComponent<Entity>();   
    }

    //Enter our starting and global states.
    void Start()
    {
        currentState = Instantiate(startingState);
        globalState = Instantiate(globalState);

        globalState.EnterState(e);
        currentState.EnterState(e);
    }

    //Calls appropriate UpdateState functions every frame.
    void Update()
    {
        //We pause the game by setting timeScale to 0, we need to ensure that our FSM also pause during this time.
        if (Time.timeScale == 0)
            return;

        globalState.UpdateState(e);
        currentState.UpdateState(e);    
    }

    //Change from our current state to a new state.
    //Calls exit functions & grabs the unloading state in case we wish to return to it.
    public void ChangeState(AbstractState nextState)
    {
        
        currentState.ExitState(e);
        previousState = Instantiate(currentState);
        currentState = Instantiate(nextState);
        currentState.EnterState(e);
    }

    //We will sometimes want to enter a state briefly, do something, and then return to 
    //the last state, whatever that may be. This function handles that.
    public void RevertToPreviousState() 
    {
        if(previousState != null)
            ChangeState(previousState);
    }
}

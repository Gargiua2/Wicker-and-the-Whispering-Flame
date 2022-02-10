using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//An abstract class defining a state in its simplest form.
//Defines the functions all states must have for our finite state machine to work.

//Inherets from ScriptableObject, so child classes can have asset menus and be created as asssets. 
//This makes them easier to work with in the inspector. 
public abstract class AbstractState : ScriptableObject
{

    //Called when a state is first entered.
    //Not all states will need an entry function, so this function
    //is virtual rather than abstract, and states can override it as neccessary.
    public virtual void EnterState(Entity e) 
    {
        
    }

    //Called every frame a state is active by the finite state machine.
    public abstract void UpdateState(Entity e);


    //Like EnterState, not every state needs an exit method, so this is virtual, not abstract.
    //Since we instantiate our states when they are swapped to, we destroy them when they are exited -
    //For this reason, all overriden base states must also call this base function.
    public virtual void ExitState(Entity e) 
    {
        Destroy(this);
    }
}

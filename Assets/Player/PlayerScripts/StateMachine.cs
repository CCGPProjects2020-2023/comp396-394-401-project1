/*
    //**Note: This referenced/from the COMP396 lecture class examples ***

    Author's Name: Alexander  Maynard
    Creation Date: October 23, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: October 24, 2023
    Program Description: This is the simple player state machine for the handling of the player ability states onEnter, onFrame and onExit

    Revision History: 
    -October 23, 2023 
        -> Created the simple state machine pattern with all methods and functions from the in class example
    -October 24, 2023 
        -> Added comment header, inline comments and descriptions for the functions
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    /// <summary>
    /// public State class that has a name for the states,\
    /// System actions for the entering (onEnter), when the state is active(onFrame) and exiting of a state(onExit).
    /// This class also contains a ToString override to the return the name of the state.
    /// </summary>
    public class State
    {
        public string Name; //Name of the state
        public System.Action onFrame; //Default action to be performed (when the state is active)
        public System.Action onEnter; //When state is entered
        public System.Action onExit; //When state is exited from 

        //toString override for the to return Name
        public override string ToString()
        {
            return Name;
        }
    }

    //PlayerStateMachine variables.

    //Dictionnary of type string and state used to hold the states of the player
    public Dictionary<string, State> states = new Dictionary<string, State>();
    //current state for the player
    public State currentState { get; private set; }
    //initial state (starting state) for the player
    public State initialState;

    //CreateState -> constructor for the states  in the PLayerStateMachine
    public State CreateState(string name)
    {
        //new state
        State state = new State();
        //set state name
        state.Name = name;

        if (states.Count == 0)
        {
            initialState = state;
        }

        states[name] = state;

        return state;
    }


    // Update is called once per frame and calls the apporpriate state checks and logic for the states. Also calls the onFrame for the states
    public void Update()
    {
        //If no states Log the error with use of the debug
        if (states.Count == 0)
        {
            Debug.LogError("*** State machine has no states! ***");
            return;
        }

        //If the current state is null then Change the state to the initial state
        if (currentState == null)
        {
            ChangeState(initialState);
        }

        //If current state onFrame(this is when the state is active) is not null then  keep current state.OnFrame 
        if (currentState.onFrame != null)
        {
            currentState.onFrame();
        }
    }

    //This methoid changes does checks for if the state is null and whether the passed state should exit or enter
    public void ChangeState(State newState)
    {
        //catch if newState is null, if so log it to the debugger with the appropriate message
        if (newState == null)
        {
            Debug.LogError("*** Can't change to a null state! ***");
            return;
        }
        //If the current state is no null and the state isn't exiting either do onExit of current state
        if (currentState != null && currentState.onExit != null)
        {
            currentState.onExit();
        }

        //Log the proper message is the debugger and then change to the newState
        Debug.LogFormat($"*** Changing from state {currentState} to state {newState} ***");
        currentState = newState;

        //If the onEnter for a state is not null then do onEnter for the currentState
        if (currentState.onEnter != null)
        {
            currentState.onEnter();
        }
    }

    //This method checks if the state contains the state with the passed state name and calls the Changestate with the states with the name that was passed.
    public void ChangeState(string newStateName)
    {
        if (states.ContainsKey(newStateName))
        {
            ChangeState(states[newStateName]);
        }
        else
        //if it doesn't contain the statename the send the appropriate message to the debugger
        {
            Debug.LogErrorFormat($"*** State machine doesn't have the state {newStateName} ***");
            return;
        }
    }
}

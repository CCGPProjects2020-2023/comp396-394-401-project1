/*
 * //---------------------------------------------------------------------------
 * //*** NOTE: THIS CODE IS MODIFIED FROM THE COMP396 CLASSWORK LAB EXAMPLES ***
 * //---------------------------------------------------------------------------
 * Author's Name:           Alexander  Maynard
 * Creation Date:           October 23, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 3, 2023
 * 
 * Program Description:     This is the simple player state machine for the handling of the 
 *                          player ability states onEnter, onFrame and onExit
 * 
 * Revision History:        October 23, 2023:
 *                              -> Created the simple state machine pattern with all methods and functions from the in class example.
 *                          
 *                          October 24, 2023:
 *                              -> Added comment header, inline comments and descriptions for the functions
 *                          
 *                          December 1, 2023:
 *                              -> Commented out the Debug.Logs -> no longer needed.
 *                          December 3, 2023:
 *                              ->Updated comments/comment headers, removed unused usings and made some variables private.
 */

using System.Collections.Generic;


/// <summary>
/// State machine class creates, changes and calls new states.
/// </summary>
public class StateMachine
{
    /// <summary>
    /// public State class that has a name for the states,
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
    private Dictionary<string, State> states = new Dictionary<string, State>();

    //current state for the player
    public State currentState { get; private set; }
    //initial state (starting state) for the player
    private State initialState;

    //CreateState -> constructor for the states  in the PLayerStateMachine
    public State CreateState(string name)
    {
        //new state
        State state = new State();
        //set state name
        state.Name = name;

        //if there are no states in the Dictionnary...
        if (states.Count == 0)
        {
            // ... then the initial state is set to the state passed in ChangeState()
            initialState = state;
        }

        //assign current state the new state passed in the ChangeState method call
        states[name] = state;

        //return the state
        return state;
    }

    /// <summary>
    /// calls the apporpriate state checks and logic for the states. Also calls the onFrame for the states
    /// </summary>
    // Update is called once per frame
    public void Update()
    {
        //If no states Log the error with use of the debug
        if (states.Count == 0)
        {
            //Debug.LogError("*** State machine has no states! ***");
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

    /// <summary>
    /// This method changes does checks for if the state is null and 
    /// whether the passed state should exit or enter
    /// </summary>
    /// <param name="newState"> newState (new state that is passed in the ChangeState call) 
    /// to be checked and then assigned if appropriate</param>
    public void ChangeState(State newState)
    {
        //catch if newState is null, if so log it to the debugger with the appropriate message
        if (newState == null)
        {
            //Debug.LogError("*** Can't change to a null state! ***");
            return;
        }
        //If the current state is no null and the state isn't exiting either do onExit of current state
        if (currentState != null && currentState.onExit != null)
        {
            currentState.onExit();
        }

        //Log the proper message is the debugger and then change to the newState
        //Debug.LogFormat($"*** Changing from state {currentState} to state {newState} ***");
        currentState = newState;

        //If the onEnter for a state is not null then do onEnter for the currentState
        if (currentState.onEnter != null)
        {
            currentState.onEnter();
        }
    }

    /// <summary>
    /// This method checks if the state contains the state with the passed state name 
    /// and calls the Changestate with the states with the name that was passed.
    /// </summary>
    /// <param name="newStateName">Name of the new state to be passed in the call to 
    /// ChangeState('new state name here')</param>
    public void ChangeState(string newStateName)
    {
        //checks if states contains a key with the newStateName string (for the states Dictionnary)
        if (states.ContainsKey(newStateName))
        {
            //if the states dictionnary contains the key then call ChangeState with the newStateName
            ChangeState(states[newStateName]);
        }
        else
        //if it doesn't contain the statename the send the appropriate message to the debugger
        {
            //Debug.LogErrorFormat($"*** State machine doesn't have the state {newStateName} ***");
            return;
        }
    }
}

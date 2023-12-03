/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 03, 2023
    Program Description:    State machine class that manages and updates the states.
    Revision History:       October 28, 2023: Initial script and documentation.
                            November 1, 2023: Added the AnimState enum.
                            November 2, 2023: Removed the singleton pattern from the state machine.
                            November 8, 2023: Added the AttackingState to the StateEnum
                            December 02, 2023: Removed Debug.Logs
                            December 03, 2023: Changed the Update() method to FixedUpdate() and added the EnragingState in the StateEnum
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    /// <summary>
    /// Enumarates the possible states of a controller.
    /// </summary>
    public enum StateEnum
    {
        RoamingState,
        LoadingState,
        ChasingState,
        ShootingState,
        AttackingState,
        EnragingState,
        EvadingState,
        DyingState
    }

    /// <summary>
    /// Abstract state class that defines the actions of a state.
    /// </summary>
    public abstract class State
    {
        protected internal EnemyController controller;
        protected internal EnemyStateMachine stateMachine;

        public string Name;
        public Action onFrame;
        public Action onEnter;
        public Action onExit;

        /// <summary>
        /// Overrides the ToString() method by specifying a string to return.
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return Name; }

        public abstract void OnEnter();
        public abstract void OnFrame();
        public abstract void OnExit();
    }

    private Dictionary<string, State> states = new();

    public State currentState { get; private set; }

    private State initialState;

    /// <summary>
    /// Adds a state to the state machine.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public State AddState(State state)
    {
        state.Name = state.GetType().Name;

        if (states.Count == 0) initialState = state;

        states[state.Name] = state;

        return state;
    }

    /// <summary>
    /// Updates the state to the state that the controller needs to change to.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void FixedUpdate()
    {
        if (states.Count == 0) throw new Exception("*** State machine has no states! ***");

        if (currentState == null) ChangeState(initialState);

        currentState.onFrame?.Invoke();
    }

    /// <summary>
    /// Specifies the next state to change to.
    /// </summary>
    /// <param name="newState"></param>
    /// <exception cref="Exception"></exception>
    public void ChangeState(State newState)
    {
        if (newState == null) throw new Exception("*** Cannot change to a null state ***");

        if (currentState != null && currentState.onExit != null) currentState.onExit();

        currentState = newState;

        currentState.onEnter?.Invoke();
    }

    /// <summary>
    /// Specifies the next state to change to.
    /// </summary>
    /// <param name="newStateEnum"></param>
    /// <exception cref="Exception"></exception>
    public void ChangeState(StateEnum newStateEnum)
    {
        if (!states.ContainsKey(newStateEnum.ToString())) throw new Exception($"*** State machine does not have the state {newStateEnum} ***");

        ChangeState(states[newStateEnum.ToString()]);
    }
}

/// <summary>
/// Animation states used in the unity animator.
/// </summary>
public enum AnimState
{
    WALKING = 1,
    LOADING = 2,
    SHOOTING = 3,
    DYING = 4
}

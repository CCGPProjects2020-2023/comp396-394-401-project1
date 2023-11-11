/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 11, 2023
    Program Description:    Dying state of a controller; specifies the
                            dying behavior.
    Revision History:       October 29, 2023: Initial script and documentation.
                            November 1, 2023: Added the animation for this state.
                            November 2, 2023: Added an EnemyStateMachine parameter to the state constructor.
                            November 11, 2023: Added a is_dead re-assigment to the DoDying function.
 */

using UnityEngine;

public class DyingState : EnemyStateMachine.State
{
    /// <summary>
    /// Initializes the action and controller.
    /// </summary>
    /// <param name="controller"></param>
    public DyingState(EnemyController controller, EnemyStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;

        onEnter = OnEnter;
        onFrame = OnFrame;
        onExit = OnExit;
    }

    /// <summary>
    /// Delegates to the OnEnter action of this state.
    /// </summary>
    public override void OnEnter() {

    }

    /// <summary>
    /// Delegates to the OnFrame action of this state - it specifies the 
    /// state transitions.
    /// </summary>
    public override void OnFrame() {
        Debug.Log("Dying state - On Frame");
        DoDying();

    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() { }

    /// <summary>
    /// Handles the dying behavior of this controller.
    /// </summary>
    private void DoDying() {
        controller.anim.SetInteger("state", (int)AnimState.DYING);
        controller.is_dead = true;
    }
}
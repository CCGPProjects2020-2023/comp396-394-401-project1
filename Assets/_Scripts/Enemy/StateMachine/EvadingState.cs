/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 03, 2023
    Program Description:    Evading state of a controller; specifies the
                            evading behavior.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the transition to the dying state.
                            November 1, 2023: Added the animation for this state.
                            November 2, 2023: Added an EnemyStateMachine parameter to the state constructor.
                            December 02, 2023: Removed Debug.Logs
                            December 03, 2023: Removed the transition to RoamingState and setting the is_attacking in the OnEnter
 */

using UnityEngine;

public class EvadingState : EnemyStateMachine.State {
    /// <summary>
    /// Initializes the action and controller.
    /// </summary>
    /// <param name="controller"></param>
    public EvadingState(EnemyController controller, EnemyStateMachine stateMachine) {
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
        controller.is_attacking = false;
    }

    /// <summary>
    /// Delegates to the OnFrame action of this state - it specifies the 
    /// state transitions.
    /// </summary>
    public override void OnFrame() {
        DoEvading();

        if (this.controller.health <= 0)
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.DyingState);
    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() { }

    /// <summary>
    /// Sets the movement of this controller when evading.
    /// </summary>
    private void DoEvading() {        
        controller.SetMovement(false);
        controller.anim.SetInteger("state", (int)AnimState.WALKING);
    }
}
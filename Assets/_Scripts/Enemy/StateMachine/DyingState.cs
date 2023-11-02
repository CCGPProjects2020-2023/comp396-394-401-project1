/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 29, 2023
    Program Description:    Dying state of a controller; specifies the
                            dying behavior.
    Revision History:       October 29, 2023: Initial script and documentation.
                            November 1, 2023: Added the animation for this state.
 */

using UnityEngine;

public class DyingState : StateMachine.State
{
    /// <summary>
    /// Initializes the action and controller.
    /// </summary>
    /// <param name="controller"></param>
    public DyingState(EnemyController controller)
    {
        this.controller = controller;

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
        //Replace this with correct behavior
        this.controller.anim.SetInteger("state", (int)AnimState.DYING);
    }
}

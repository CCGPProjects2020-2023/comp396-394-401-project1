/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 29, 2023
    Program Description:    Chasing state of a controller; specifies the
                            chasing behavior.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the transition to the dying state.
                            November 1, 2023: Adjusted the transition to the shooting state.
                                              Added the animation for this state.
 */

using UnityEngine;

public class ChasingState : StateMachine.State {

    /// <summary>
    /// Initializes the action and controller.
    /// </summary>
    /// <param name="controller"></param>
    public ChasingState(EnemyController controller) {
        this.controller = controller;

        onEnter = OnEnter;
        onFrame = OnFrame;
        onExit = OnExit;
    }

    /// <summary>
    /// Delegates to the OnEnter action of this state.
    /// </summary>
    public override void OnEnter() { }

    /// <summary>
    /// Delegates to the OnFrame action of this state - it specifies the chasing behavior and
    /// the state transitions.
    /// </summary>
    public override void OnFrame() {
        Debug.Log("Chasing state - On Frame");
        DoChasing();
        
        if (this.controller.health <= 0)
            stateMachine.ChangeState(StateMachine.StateEnum.DyingState);

        if (!controller.SensePlayer())
            stateMachine.ChangeState(StateMachine.StateEnum.RoamingState);

        else if (controller.WithinRange())
            stateMachine.ChangeState(StateMachine.StateEnum.ShootingState);
    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() { }

    private void DoChasing() {        
        controller.SetMovement(true);
        this.controller.anim.SetInteger("state", (int)AnimState.WALKING);
    }
}
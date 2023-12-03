/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 02, 2023
    Program Description:    Loading state of a controller; specifies the
                            loading behavior.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the transition to the dying state.
                            November 1, 2023: Added the animation for this state.
                            November 2, 2023: Added an EnemyStateMachine parameter to the state constructor.
                            November 8, 2023: Added differentiation for the specific controllers.
                            December 02, 2023: Removed Debug.Logs
 */

using UnityEngine;

public class LoadingState : EnemyStateMachine.State {
    /// <summary>
    /// Initializes the action and controller.
    /// </summary>
    /// <param name="controller"></param>
    public LoadingState(EnemyController controller, EnemyStateMachine stateMachine) {
        this.controller = controller;
        this.stateMachine = stateMachine;

        onEnter = OnEnter;
        onFrame = OnFrame;
        onExit = OnExit;
    }

    /// <summary>
    /// Delegates to the OnEnter action of this state.
    /// </summary>
    public override void OnEnter() { }

    /// <summary>
    /// Delegates to the OnFrame action of this state - it specifies the 
    /// state transitions.
    /// </summary>
    public override void OnFrame() {      
        DoLoading();

        if (this.controller.health <= 0)
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.DyingState);

        if (!controller.SensePlayer())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.RoamingState);

        else if (controller is ShooterController && (controller as ShooterController).IsWeaponReady() && controller.SensePlayer())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.ChasingState);
    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() { }

    /// <summary>
    /// Checks if the weapon of this controller is ready. If not, loads it.
    /// </summary>
    private void DoLoading() {
        if (controller is ShooterController && !(controller as ShooterController).IsWeaponReady()) {
            controller.anim.SetInteger("state", (int)AnimState.LOADING);
            controller.weapon.Load_Weapon();
        }            
    }
}
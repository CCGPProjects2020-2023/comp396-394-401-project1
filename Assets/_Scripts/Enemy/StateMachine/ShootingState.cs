/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 02, 2023
    Program Description:    Shooting state of a controller; specifies the
                            shooting behavior.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the transition to the dying state.
                            November 1, 2023: Added the Rotate() and the animation for this state.
                            November 2, 2023: Added an EnemyStateMachine parameter to the state constructor.
                            November 8, 2023: Added differentiation for the specific controllers.
                            November 11, 2023: Removed the Rotate() function.
                            December 02, 2023: Changed the way the weapon is being activated and deactivated and removed Debug.Logs
 */

using UnityEngine;

public class ShootingState : EnemyStateMachine.State {


    /// <summary>
    /// Initializes the action and controller.
    /// </summary>
    /// <param name="controller"></param>
    public ShootingState(EnemyController controller, EnemyStateMachine stateMachine) {
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
    /// Delegates to the OnFrame action of this state  - it specifies the 
    /// state transitions from this state.
    /// </summary>
    public override void OnFrame() {
        DoShooting();

        if (!controller.SensePlayer())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.RoamingState);

        else if (controller is ShooterController && !(controller as ShooterController).IsWeaponReady())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.LoadingState);

        else if (!controller.WithinRange() && controller.SensePlayer())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.ChasingState);

        else if (Utils.IsBelowThreshold(controller._start_health / 4, controller.health))
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.EvadingState);
    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() { 
        controller.weapon.Deactivate();
    }

    /// <summary>
    /// Calls the shoot method of the weapon of this controller.
    /// </summary>
    private void DoShooting() {
        controller.anim.SetInteger("state", (int)AnimState.SHOOTING);
        controller.transform.LookAt(controller.player.transform.position);
        
        if(!controller.weapon.isActivated)
            controller.weapon.Activate();        
    }
}
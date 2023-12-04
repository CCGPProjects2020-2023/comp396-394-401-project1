/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 03, 2023
    Program Description:    Enraging state of a controller; specifies the enrage behavior.
    Revision History:       December 03, 2023: Initial script and documentation.                            
 */

using UnityEngine;

public class EnragingState : EnemyStateMachine.State
{
    public EnragingState(EnemyController controller, EnemyStateMachine stateMachine)
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
        controller.EnemyFOV = 360;
        controller.cosEnemyFOVover2InRAD = Mathf.Cos(controller.EnemyFOV / 2f * Mathf.Deg2Rad); ;
        controller.closeEnoughEngageCutoff *= 4;
    }

    /// <summary>
    /// Delegates to the OnFrame action of this state  - it specifies the 
    /// state transitions from this state.
    /// </summary>
    public override void OnFrame()
    {
        DoEnraging();

        if (controller.health <= 0)
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.DyingState);
        
        else if (Utils.IsBelowThreshold(controller._start_health / 6, controller.health))
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.EvadingState);
    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() {
        if (controller is ShooterController)        
            controller.weapon.Deactivate();        

        controller.is_attacking = false;
    }

    /// <summary>
    /// Sets the proper animation and behavior for that state.
    /// </summary>
    private void DoEnraging() {
        controller.SetMovement(true);
        if (controller is CloseRangedController && !controller.is_attacking)
            (controller as CloseRangedController).DoAttack(false);        

        else if(!controller.is_attacking) {
            controller.anim.SetInteger("state", (int)AnimState.WALKING);

            if (!controller.weapon.isActivated) controller.weapon.Activate();
        }
    }
}
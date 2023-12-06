/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     Decmeber 03, 2023
    Program Description:    Attacking state of a controller; specifies the
                            attacking behavior.
    Revision History:       November 8, 2023: Initial script and documentation.
                            December 02, 2023: Playing and stopping the controller's audio clip and removed Debug.Logs
                            December 03, 2023: Added access to the clips list and playing the appropriate sound when attacking state 
                            and multiplied the threshold to transition to evading state by 1/2. Also added the transition to the EnragingState
 */

public class AttackingState : EnemyStateMachine.State
{
    /// <summary>
    /// Initializes the action and controller.
    /// </summary>
    /// <param name="controller"></param>
    public AttackingState(EnemyController controller, EnemyStateMachine stateMachine)
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
    public override void OnEnter() { }

    /// <summary>
    /// Delegates to the OnFrame action of this state  - it specifies the 
    /// state transitions from this state.
    /// </summary>
    public override void OnFrame()
    {
        DoAttacking();

        if (controller.health <= 0)
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.DyingState);

        if (!controller.SensePlayer())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.RoamingState);       

        else if (!controller.WithinRange() && controller.SensePlayer())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.ChasingState);

        else if (Utils.IsBelowThreshold(controller._start_health / 2, controller.health))
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.EnragingState);
    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() { }

    /// <summary>
    /// Sets the proper animation for that state.
    /// </summary>
    private void DoAttacking()
    {
        controller.is_attacking = true;
        (controller as CloseRangedController).DoAttack(false);
    }
}
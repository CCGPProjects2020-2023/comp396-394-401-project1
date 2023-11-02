/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 29, 2023
    Program Description:    Shooting state of a controller; specifies the
                            shooting behavior.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the transition to the dying state.
                            November 1, 2023: Added the Rotate() and the animation for this state.
 */

using UnityEngine;

public class ShootingState : EnemyStateMachine.State {

    private bool hasRotated = false;

    /// <summary>
    /// Initializes the action and controller.
    /// </summary>
    /// <param name="controller"></param>
    public ShootingState(EnemyController controller) {
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
    /// Delegates to the OnFrame action of this state  - it specifies the 
    /// state transitions from this state.
    /// </summary>
    public override void OnFrame() {
        Debug.Log("Shooting State - On Frame");
        DoShooting();

        if (!controller.SensePlayer())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.RoamingState);

        else if (!controller.IsWeaponReady())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.LoadingState);

        else if (!controller.WithinRange() && controller.SensePlayer())
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.ChasingState);

        else if (Utils.IsBelowThreshold(controller._start_health / 2, controller.health))
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.EvadingState);
    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() { hasRotated = false; }

    /// <summary>
    /// Calls the shoot method of the weapon of this controller.
    /// </summary>
    private void DoShooting() {
        controller.anim.SetInteger("state", (int)AnimState.SHOOTING);
        if(!hasRotated )
            Rotate();
        
        controller.weapon.Shoot();        
    }
     
    /// <summary>
    /// Rotates this controller to face the player.
    /// </summary>
    private void Rotate() {
        Vector3 controllerVec = controller.transform.position;
        Vector3 weaponTipVec = controller.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).transform.position;

        float a = Vector3.Angle(controllerVec, weaponTipVec);
        Vector3 newRotation = new(controller.transform.rotation.x, controller.transform.rotation.y + a, controller.transform.rotation.z);
        controller.transform.Rotate(newRotation);
        
        hasRotated = true;
    }
}
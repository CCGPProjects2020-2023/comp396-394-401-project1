/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 28, 2023
    Program Description:    Roaming state of a controller; specifies the path and 
                            roaming behavior.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the transition to the dying state.
                            November 1, 2023: Accomodated the changes made in the EnemyController on November 1, 2023.
                                              Added the animation for this state.
 */

using UnityEngine;
using System;

public class RoamingState : EnemyStateMachine.State {

    /// <summary>
    /// Initializes the action and controller.
    /// </summary>
    /// <param name="controller"></param>
    public RoamingState(EnemyController controller) {
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
    /// Delegates to the OnFrame action of this state - it specifies the roaming behavior and
    /// the state transitions from this state.
    /// </summary>
    public override void OnFrame() {
        Debug.Log("Roaming state - On Frame");        
        DoRoaming();

        if (this.controller.health <= 0)
            stateMachine.ChangeState(EnemyStateMachine.StateEnum.DyingState);

        if (this.controller.SensePlayer() && !Utils.IsBelowThreshold(controller._start_health / 2, controller.health))
        {
            if (!this.controller.IsWeaponReady())
                stateMachine.ChangeState(EnemyStateMachine.StateEnum.LoadingState);
            else
                stateMachine.ChangeState(EnemyStateMachine.StateEnum.ChasingState);
        }
    }

    /// <summary>
    /// Specifies the roaming behavior of this controller based on a predetermined path.
    /// </summary>
    /// <exception cref="Exception"></exception>
    void DoRoaming() {        
        if (controller.path.transform.childCount == 0) throw new Exception("Insert waypoints");
        
        if (Vector3.Distance(controller.transform.position, controller.path.transform.GetChild(controller.nextWayPointIndex).position) < float.Epsilon)
            controller.nextWayPointIndex = (controller.nextWayPointIndex + 1) % controller.path.transform.childCount;

        Vector3 target = controller.path.transform.GetChild(controller.nextWayPointIndex).position;
        Vector3 movement = Vector3.MoveTowards(controller.transform.position, target, controller.speed * Time.deltaTime);
        controller.transform.position = movement;
        controller.transform.LookAt(target);        
        this.controller.anim.SetInteger("state", (int)AnimState.WALKING);
    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() { }
}
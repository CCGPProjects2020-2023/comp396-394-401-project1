/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 28, 2023
    Program Description:    Roaming state of a controller; specifies the path and 
                            roaming behavior.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the transition to the dying state.
 */

using UnityEngine;
using System;

public class RoamingState : StateMachine.State {

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
            stateMachine.ChangeState(StateMachine.StateEnum.DyingState);

        if (this.controller.SensePlayer() && !Utils.IsBelowThreshold(this.controller._start_health / 2, this.controller.health))
        {
            if (!this.controller.IsWeaponReady())
                stateMachine.ChangeState(StateMachine.StateEnum.LoadingState);
            else
                stateMachine.ChangeState(StateMachine.StateEnum.ChasingState);
        }
    }

    /// <summary>
    /// Specifies the roaming behavior of this controller based on a predetermined path.
    /// </summary>
    /// <exception cref="Exception"></exception>
    void DoRoaming() {
        if (this.controller.waypoints.Length == 0) throw new Exception("Insert waypoints");

        if (Vector3.Distance(this.controller.transform.position, this.controller.waypoints[this.controller.nextWayPointIndex].position) < float.Epsilon)
            this.controller.nextWayPointIndex = (this.controller.nextWayPointIndex + 1) % this.controller.waypoints.Length;

        Vector3 target = this.controller.waypoints[this.controller.nextWayPointIndex].position;
        Vector3 movement = Vector3.MoveTowards(this.controller.transform.position, target, this.controller.speed * Time.deltaTime);
        this.controller.transform.position = movement;
        this.controller.transform.LookAt(target);
    }

    /// <summary>
    /// Delegates to the OnExit action of this state.
    /// </summary>
    public override void OnExit() { }
}
/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 11, 2023
    Program Description:    Subclass that specifies specific types of enemies.
    Revision History:       November 8, 2023: Initial script and documentation.
                            November 11, 2023: Changed the modifier of the Start function and added a check to see if controller is dead.
 */

using UnityEngine;

public class CloseRangedController : EnemyController
{
    /// <summary>
    /// Start method called by Unity. It initializes the states
    /// and properties of this controller.
    /// </summary>
    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();

        stateMachine.AddState(new RoamingState(this, stateMachine));       
        stateMachine.AddState(new ChasingState(this, stateMachine));
        stateMachine.AddState(new AttackingState(this, stateMachine));
        stateMachine.AddState(new EvadingState(this, stateMachine));
        stateMachine.AddState(new DyingState(this, stateMachine));
    }

    new void Update() {
        base.Update();
        if (is_dead)
            Destroy(gameObject, 5);
    }
}
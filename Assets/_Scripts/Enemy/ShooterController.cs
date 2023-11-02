/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 29, 2023
    Program Description:    Subclass that specifies specific types of enemies.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the dying state.
                            November 1, 2023: Initialized the anim property.
 */

using UnityEngine;

public class ShooterController : EnemyController {
    /// <summary>
    /// Start method called by Unity. It initializes the states
    /// and properties of this controller.
    /// </summary>
    new void Start() {
        base.Start();
        anim = GetComponent<Animator>();

        stateMachine.AddState(new RoamingState(this));
        stateMachine.AddState(new LoadingState(this));
        stateMachine.AddState(new ChasingState(this));
        stateMachine.AddState(new ShootingState(this));
        stateMachine.AddState(new EvadingState(this));
        stateMachine.AddState(new DyingState(this));
    }
    
    /// <summary>
    /// Checks if the weapon of this controller is ready to be used.
    /// </summary>
    /// <returns>
    ///     True if the weapon is ready.
    /// </returns>
    protected internal override bool IsWeaponReady() {
        weapon.isLoaded = weapon.numbAmmo > 0;

        return weapon.isLoaded;
    }   
}
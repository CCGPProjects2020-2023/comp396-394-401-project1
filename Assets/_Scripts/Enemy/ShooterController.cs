/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 11, 2023
    Program Description:    Subclass that specifies specific types of enemies.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the dying state.
                            November 1, 2023: Initialized the anim property.
                            November 2, 2023: Added the statemachine variable to the states' constructors.
                            November 8, 2023: Removed the override modifier from the IsWeaponReady() method.
                            November 11, 2023: Changed the modifier of the Start function and added a check to see if controller is dead.
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

        stateMachine.AddState(new RoamingState(this, stateMachine));
        stateMachine.AddState(new LoadingState(this, stateMachine));
        stateMachine.AddState(new ChasingState(this, stateMachine));
        stateMachine.AddState(new ShootingState(this, stateMachine));
        stateMachine.AddState(new EvadingState(this, stateMachine));
        stateMachine.AddState(new DyingState(this, stateMachine));
    }

    new void Update() {
        base.Update();
        if(is_dead)        
            Destroy(gameObject, 5);        
    }

    /// <summary>
    /// Checks if the weapon of this controller is ready to be used.
    /// </summary>
    /// <returns>
    ///     True if the weapon is ready.
    /// </returns>
    protected internal bool IsWeaponReady() {
        weapon.isLoaded = weapon.numbAmmo > 0;

        return weapon.isLoaded;
    }   
}
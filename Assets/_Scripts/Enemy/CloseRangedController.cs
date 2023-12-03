/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 03, 2023
    Program Description:    Subclass that specifies specific types of enemies.
    Revision History:       November 8, 2023: Initial script and documentation.
                            November 11, 2023: Changed the modifier of the Start function and added a check to see if controller is dead.
                            December 02, 2023: Changed to FixedUpdate() and added an audio component
                            December 03, 2023: Removed the audio component and moved it to parent. Added DoAttack() method.
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
        stateMachine.AddState(new EnragingState(this, stateMachine));
        stateMachine.AddState(new EvadingState(this, stateMachine));
        stateMachine.AddState(new DyingState(this, stateMachine));
    }

    /// <summary>
    /// FixedUpdate function called by unity
    /// </summary>
    new void FixedUpdate() {
        base.FixedUpdate();
        if (is_dead)
            Destroy(gameObject, 5);
    }

    /// <summary>
    /// Takes care of the attack behavior for this type of controller.
    /// </summary>
    /// <param name="with_sound"></param>
    public void DoAttack(bool with_sound) {
        anim.SetInteger("state", (int)AnimState.SHOOTING);
        weapon.Activate();

        if (with_sound && anim.GetCurrentAnimatorStateInfo(0).IsName("super punch") && audio.isPlaying) audio.PlayOneShot(clips[2]);

        if (Vector3.Distance(transform.position, player.transform.position) < 2f) {
            Vector3 direction = (transform.position - player.transform.position).normalized;
            Vector3 newPosition = player.transform.position + direction * 2f;
            transform.position = newPosition;
        }            
    }
}
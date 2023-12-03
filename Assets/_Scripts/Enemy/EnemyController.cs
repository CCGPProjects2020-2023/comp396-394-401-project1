/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 02, 2023
    Program Description:    Abstract class used as a base for the different types of enemies.
    Revision History:       October 28, 2023: Initial script and documentation.
                            November 1, 2023: Changed the waypoints Transform[] property to a GameObject path property   
                                              Added the anim property.
                            November 2, 2023: Removed the singleton instance of the EnemyStateMachine and used the new() operator instead.
                            November 8, 2023: Removed the IsWeaponReady() method since it is specific to the ShooterController.
                            November 11, 2023: Changed the modifier of the start function and added a comment to it. 
                            November 21, 2023: Added the slider.
                            December 02, 2023: Changed to FixedUpdate()
 */

using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour {

    protected internal EnemyStateMachine stateMachine;
    protected internal Animator anim; 

    public GameObject player;
    public Slider health_bar;

    [Header("Internal Properties")]
    public float EnemyFOV = 89f;
    protected internal float cosEnemyFOVover2InRAD;
    public float closeEnoughEngageCutoff = 30f;
    public float closeEnoughSenseCutoff = 45f;
    protected internal bool is_dead = false;

    [Header("Game Properties")]
    protected internal float _start_health;
    public float health = 100f;
    public float speed = 2f;
    public float strenght = 90f;

    [Header("Weapon")]
    public Weapon weapon;

    [Header("Path")]
    public GameObject path;
    public int nextWayPointIndex = 0;

    /// <summary>
    /// Awake method called by Unity. It initiates the Singleton instance of the state machine.
    /// </summary>
    private void Awake() {
        stateMachine = new();
    }

    /// <summary>
    /// Start function called by Unity. It sets values for private variables.
    /// </summary>
    public void Start() { //
        cosEnemyFOVover2InRAD = Mathf.Cos(EnemyFOV / 2f * Mathf.Deg2Rad);
        _start_health = health;      
    }

    /// <summary>
    /// Update method called by Unity. It calles the update method of the state machine.
    /// </summary>
    public void FixedUpdate() {
        stateMachine.Update();
        health_bar.value = health;
    }

    /// <summary>
    /// Method that checks if the controller senses the player.
    /// </summary>
    /// <returns></returns>
    protected internal bool SensePlayer() {
        return Utils.SenseOther(gameObject, player, cosEnemyFOVover2InRAD, closeEnoughSenseCutoff);
    }

    /// <summary>
    /// Sets the movement of the controller.
    /// </summary>
    /// <param name="isFollowing"></param>
    protected internal void SetMovement(bool isFollowing) {        
        Utils.Movement(isFollowing, gameObject, player, out Vector3 newPos, speed);
        transform.position = newPos;
    }

    /// <summary>
    /// Checks if this controller is within range of the player.
    /// </summary>
    /// <returns></returns>
    protected internal bool WithinRange() {
        return Utils.OtherCloseEnough(closeEnoughEngageCutoff, gameObject, player);
    }
}
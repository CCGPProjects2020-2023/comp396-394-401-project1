/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 28, 2023
    Program Description:    Abstract class used as a base for the different types of enemies.
    Revision History:       October 28, 2023: Initial script and documentation.
 */

using UnityEngine;

public abstract class EnemyController : MonoBehaviour {

    protected StateMachine stateMachine;

    public GameObject player;

    [Header("Internal Properties")]
    public float EnemyFOV = 89f;
    protected internal float cosEnemyFOVover2InRAD;
    public float closeEnoughEngageCutoff = 10f;
    public float closeEnoughSenseCutoff = 15f;

    [Header("Game Properties")]
    protected internal float _start_health;
    public float health = 100f;
    public float speed = 2f;
    public float strenght = 90f;

    [Header("Weapon")]
    public Weapon weapon;

    [Header("Path")]
    public Transform[] waypoints;
    public int nextWayPointIndex = 0;

    /// <summary>
    /// Awake method called by Unity. It initiates the Singleton instance of the state machine.
    /// </summary>
    private void Awake() {
        stateMachine = StateMachine.Instance;
    }

    /// <summary>
    /// Update method called by Unity. It calles the update method of the state machine.
    /// </summary>
    private void Update() {
        stateMachine.Update();
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
        this.transform.position = newPos;
    }

    /// <summary>
    /// Checks if this controller is within range of the player.
    /// </summary>
    /// <returns></returns>
    protected internal bool WithinRange() {
        return Utils.OtherCloseEnough(closeEnoughEngageCutoff, gameObject, player);
    }

    /// <summary>
    /// Abstract method that is meant to check if the weapon is ready to be fired.
    /// </summary>
    /// <returns></returns>
    protected internal abstract bool IsWeaponReady();
}
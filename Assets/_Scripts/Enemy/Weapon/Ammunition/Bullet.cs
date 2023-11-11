/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 8, 2023
    Program Description:    Subclass of the Ammunition abstract class; this is to specify
                            properties and methods of the bullet object instantiated by a weapon.
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Added the start and destroy method to remove unused bullets.
                            November 8, 2023: Removed the OnTrigger() method and moved it to its super class.
 */


using System.Collections;
using UnityEngine;

public class Bullet : Ammunition
{
    /// <summary>
    /// Start method called by unity. This method ensures that the start method of the base
    /// class is also called. It starts the coroutine of the destruction of this bullet.
    /// </summary>
    private new void Start()
    {
        base.Start();
        StartCoroutine(Destroy(6f));
    }

    /// <summary>
    /// Update method called by Unity once per frame.
    /// </summary>
    void Update()
    {
        SetMovement(this.movement);
    }

    /// <summary>
    /// Sets the direction of the bullet when instantiated.
    /// </summary>
    /// <param name="movement">
    ///     A vector representing the direction towards which this object will be 
    ///     moving to.
    /// </param>
    private void SetMovement(Vector3 movement)
    {
        transform.position += movement;
    }

    /// <summary>
    /// Destroy this object after a certain amount of time if it still exists.
    /// </summary>
    /// <param name="wait_time"></param>
    /// <returns></returns>
    private IEnumerator Destroy(float wait_time)
    {
        yield return new WaitForSeconds(wait_time);
        Destroy(gameObject);
    }
}
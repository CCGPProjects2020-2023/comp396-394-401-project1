/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 28, 2023
    Program Description:    Abstract class used as a base to other weapon types. 
    Revision History:       October 28, 2023: Initial script and documentation.
 */

using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [Header("Ammunition Info")]
    public int numberOfAmmoPerRound = 20;    
    protected internal Vector3 ammoSpawnLocation;     
    protected internal int currentCount = 0;   
    protected internal int numbAmmo = 0;
    [SerializeField]
    protected internal float spawnRatePerMinute = 30;

    protected internal AmmunitionFactory factory;

    [Header("Weapon Properties")]
    protected internal bool isLoaded = false;        
    public float loadingTime = 2f;

    /// <summary>
    /// Start method called by Unity that initializes the Ammunition Factory.
    /// </summary>
    private void Start() {
        factory = gameObject.GetComponent<AmmunitionFactory>();
    }

    /// <summary>
    /// Starts a timer that resets the weapon to a full round of ammo.
    /// </summary>
    protected internal void Load_Weapon() {
        StartCoroutine(Load());
    }

    /// <summary>
    /// Sets the number of ammo available to this weapon to the max.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Load() {
        yield return new WaitForSeconds(this.loadingTime);
        numbAmmo = numberOfAmmoPerRound;
    }

    /// <summary>
    /// Abstract method that is to be overwritten by the subclasses of this class.
    /// This method is meant to implement the shoot function of a weapon type.
    /// </summary>
    public abstract void Shoot();
}
/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 02, 2023
    Program Description:    Abstract class used as a base to other weapon types. 
    Revision History:       October 28, 2023: Initial script and documentation.
                            November 8, 2023: Changed the Start() method modifier to protected internal
`                           November 21, 2023: Changed the initial number of the currentCount from 0 to 20;
                            December 02, 2023: Added the audio and isActivated properties, as well as the Deactivate() method.
 */

using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Ammunition Info")]
    public int numberOfAmmoPerRound = 20;
    protected internal Vector3 ammoSpawnLocation;
    protected internal int currentCount = 20;
    protected internal int numbAmmo = 0;
    [SerializeField]
    protected internal float spawnRatePerMinute = 30;

    protected internal AmmunitionFactory factory;

    [Header("Weapon Properties")]
    protected internal bool isLoaded = false;
    public float loadingTime = 2f;

    protected internal new AudioSource audio;

    protected internal bool isActivated = false;

    /// <summary>
    /// Start method called by Unity that initializes the Ammunition Factory.
    /// </summary>
    protected internal void Start()
    {
        audio = GetComponent<AudioSource>();
        factory = gameObject.GetComponent<AmmunitionFactory>();
    }

    /// <summary>
    /// Starts a timer that resets the weapon to a full round of ammo.
    /// </summary>
    protected internal void Load_Weapon()
    {
        StartCoroutine(Load());
    }

    /// <summary>
    /// Sets the number of ammo available to this weapon to the max.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Load()
    {
        yield return new WaitForSeconds(this.loadingTime);
        numbAmmo = numberOfAmmoPerRound;
    }

    /// <summary>
    /// Abstract methods that are to be overwritten by the subclasses of this class.
    /// These methods are meant to implement the shoot function of a weapon type.
    /// </summary>
   public abstract void Activate();
   public abstract void Deactivate();
}
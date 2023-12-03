/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 02, 2023
    Program Description:    Subclass of the abstract Weapon class. This is to specify the behaviors
    Revision History:       October 28, 2023: Initial script and documentation.
                            November 8, 2023: Renamed function Shoot() to Activate()
                            December 02, 2023: Added a Start() method, a Deactivate() method, a To_be_performed() method and modified the Activate() method.
 */

using UnityEngine;

public class MachineGun : Weapon
{
    private ParticleSystem particle;

    /// <summary>
    /// Unity's start function - calls the parents and initialize the particle system.
    /// </summary>
    private new void Start()
    {
        base.Start();
        particle = transform.GetChild(1).GetComponent<ParticleSystem>();       
    }

    /// <summary>
    /// This method uses the factory class to instantiate and shoot ammunitions.
    /// </summary>
    public override void Activate()
    {
        InvokeRepeating("To_be_performed", 0f, 0.5f);
        isActivated = true;
    }

    public override void Deactivate() {
        CancelInvoke("To_be_performed");
        isActivated= false;
    }

    private void To_be_performed() {
        factory.GetNewInstance(gameObject.transform.GetChild(0).transform.position);
        audio.PlayOneShot(audio.clip);
        if (!particle.isPlaying) particle.Play();

        currentCount++;
        numbAmmo--;
    }
}

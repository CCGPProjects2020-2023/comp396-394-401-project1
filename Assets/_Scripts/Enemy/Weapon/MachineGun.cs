/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 28, 2023
    Program Description:    Subclass of the abstract Weapon class. This is to specify the behaviors
    Revision History:       October 28, 2023: Initial script and documentation.
 */

using UnityEngine;

public class MachineGun : Weapon
{
    /// <summary>
    /// This method uses the factory class to instantiate and shoot ammunitions.
    /// </summary>
    public override void Shoot()
    {
        var targetCount = Time.time * (spawnRatePerMinute / 60);
        while (targetCount > currentCount && numbAmmo > 0)
        {
            factory.GetNewInstance(gameObject.transform.GetChild(0).transform.position);

            currentCount++;
            numbAmmo--;
        }
    }
}

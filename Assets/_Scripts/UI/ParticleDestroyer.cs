/*
 * Author's Name:           Alexander  Maynard
 * Creation Date:           November 28, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 3, 2023
 * 
 * Program Description:     This script destroys the instantiated abilities particle effects 
 *                          (that happen when player abilities are used).
 *                          
 * Revision History:        November 28, 2023:
 *                              -> Created simple particle destroyer script for the abilitites particle effect.
 *                              
 *                          December 3, 2023:
 *                              -> Added comments/comment headers to this script.
 */

using UnityEngine;

/// <summary>
/// This class destroys the particles for the abilities when they should be done executing.
/// </summary>
public class ParticleDestroyer : MonoBehaviour
{
    //On start (when the particles are first instantiated) call Destroy
    // Start is called before the first frame update
    void Start()
    {
        //call Destroy with a delay of 1 second as we want the particles to only display for roughly 1 second.
        Destroy(this.gameObject, 1);
    }
}

/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles the destruction of an object
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube

using System.Collections;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    public float lifeTime = 1.5f;

    /// <summary>
    /// Destroys the game object that has this script attached to.
    /// </summary>
    /// <returns></returns>
    IEnumerator Start() { 
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
}
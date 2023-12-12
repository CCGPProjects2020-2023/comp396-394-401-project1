/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Utility for Network Objects
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using UnityEngine;

public static class NetworkUtils
{
    /// <summary>
    /// Returns a random Vector3 used to spawn the player at a random place
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetRandomSpawnPoint() {
        return new Vector3(Random.Range(-150, -130), 4, Random.Range(10, 20));
    }

    /// <summary>
    /// Sets the layers of children in an object
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="layerNumber"></param>
    public static void SetRenderLayerInChildren(Transform transform, int layerNumber) {
        foreach (Transform trans in transform.GetComponentsInChildren<Transform>(true)) {
            if(trans.CompareTag("IgnoreLayerChange")) continue;

            trans.gameObject.layer = layerNumber;
        }        
    }
}
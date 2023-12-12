/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles the start up of network objects. 
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using UnityEngine;

public class StartUp
{
    /// <summary>
    /// Retrieves all static resources and instantiate them
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitiatePrefabs() {
        Debug.Log("-- Instantiating objects --");
        GameObject[] prefabsToInstantiate = Resources.LoadAll<GameObject>("InstantiateOnLoad/");

        foreach (GameObject pref in prefabsToInstantiate) {
            Debug.Log($"Creating {pref.name}");
            GameObject.Instantiate(pref);
        }
        Debug.Log("-- Instantiating objects done --");
    }
}
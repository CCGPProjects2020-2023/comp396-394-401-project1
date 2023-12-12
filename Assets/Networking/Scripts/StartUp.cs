using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp
{
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

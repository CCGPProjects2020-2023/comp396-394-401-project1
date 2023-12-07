using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkUtils
{
    public static Vector3 GetRandomSpawnPoint() {
        return new Vector3(Random.Range(-150, -130), 4, Random.Range(10, 20));
    }

    public static void SetRenderLayerInChildren(Transform transform, int layerNumber) {
        foreach (Transform trans in transform.GetComponentsInChildren<Transform>(true)) {
            if(trans.CompareTag("IgnoreLayerChange")) continue;

            trans.gameObject.layer = layerNumber;
        }
        


    }
}
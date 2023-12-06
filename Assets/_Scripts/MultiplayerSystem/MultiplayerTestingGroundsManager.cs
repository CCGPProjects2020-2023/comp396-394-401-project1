/** Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     December 5, 2023
 *  Program Description:    Manages the level.
 *  Revision History:       December 5, 2023 (Marcus Ngooi): Initial MultiplayerTestingGroundsManager script.
 */

using Unity.Netcode;
using UnityEngine;

    public class MultiplayerTestingGroundsManager : MonoBehaviour
    {
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                //SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        //static void SubmitNewPosition()
        //{
        //    if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
        //    {
        //        if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
        //        {
        //            foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
        //                NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
        //        }
        //        else
        //        {
        //            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        //            var player = playerObject.GetComponent<HelloWorldPlayer>();
        //            player.Move();
        //        }
        //    }
        //}
    }

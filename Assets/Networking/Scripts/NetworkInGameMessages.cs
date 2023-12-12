/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles the in game messages on a network
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using UnityEngine;
using Fusion;

public class NetworkInGameMessages : NetworkBehaviour
{
    InGameMessageUIHandler inGameMessageUIHandler;
    
    /// <summary>
    /// Handles the submission of an in game message
    /// </summary>
    /// <param name="userNickName"></param>
    /// <param name="message"></param>
    public void SendInGameRPCMessage(string userNickName, string message) {
        RPC_InGameMessage($"<b>{userNickName}</b> {message}");
    }


    /// <summary>
    /// Handles RPC's in game messages
    /// </summary>
    /// <param name="message"></param>
    /// <param name="info"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_InGameMessage(string message, RpcInfo info = default) {
        Debug.Log($"[RPC] InGameMessage {message}");

        if(inGameMessageUIHandler == null) { 
            inGameMessageUIHandler = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<InGameMessageUIHandler>();
        }

        if(inGameMessageUIHandler != null)
        {
            inGameMessageUIHandler.OnGameMessageReceived(message);
        }
    }
}
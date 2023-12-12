/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Spawns network objects
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPlayer playerPrefab;

    Dictionary<int, NetworkPlayer> mapTokenIDWithNetworkPlayer;

    CharacterInputHandler characterInputHandler;

    SessionListUIHandler sessionListUIHandler;

    /// <summary>
    /// Awake method called by unity - Initializes properties.
    /// </summary>
    void Awake() { 
        mapTokenIDWithNetworkPlayer = new Dictionary<int, NetworkPlayer>();
        sessionListUIHandler = FindObjectOfType<SessionListUIHandler>(true);
    }

    /// <summary>
    /// Return the player token
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    int GetPlayerToken(NetworkRunner runner, PlayerRef player) {
        if (runner.LocalPlayer == player)
        {
            return ConnectionTokenUtils.HashToken(GameManager.instance.GetConnectionToken());
        }
        else { 
            var token = runner.GetPlayerConnectionToken(player);

            if(token != null) {
                return ConnectionTokenUtils.HashToken(token);
            }

            Debug.LogError($"GetPlayerToken return invalid token");

            return 0;
        }        
    }

    /// <summary>
    /// Debugs when connected to server
    /// </summary>
    /// <param name="runner"></param>
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");
    }

    /// <summary>
    /// Adds the token to a dictionary
    /// </summary>
    /// <param name="token"></param>
    /// <param name="networkPlayer"></param>
    public void SetconnectionTokenMapping(int token, NetworkPlayer networkPlayer) { 
        mapTokenIDWithNetworkPlayer.Add(token, networkPlayer);
    }

    /// <summary>
    /// Handles actions when the player joins a session
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            int playerToken = GetPlayerToken(runner, player);

            Debug.Log($"OnPlayerJoined we are server. Connection token {playerToken}");

            if (mapTokenIDWithNetworkPlayer.TryGetValue(playerToken, out NetworkPlayer networkPlayer))
            {
                Debug.Log($"Found old connection token for token {playerToken}. Assigning controls to that player");

                networkPlayer.GetComponent<NetworkObject>().AssignInputAuthority(player);
                networkPlayer.Spawned();
            }
            else {
                Debug.Log($"Spawning new player for connection token {playerToken}");
                NetworkPlayer spawnedNetworkPlayer = runner.Spawn(playerPrefab, NetworkUtils.GetRandomSpawnPoint(), Quaternion.identity, player);

                spawnedNetworkPlayer.token = playerToken;
                mapTokenIDWithNetworkPlayer[playerToken] = spawnedNetworkPlayer;
            }            
        }
        else {
            Debug.Log("OnPlayerjoined");
        }
    }

    /// <summary>
    /// Handles actions when an input is registered
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (characterInputHandler == null && NetworkPlayer.Local != null) { 
            characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();
        }

        if(characterInputHandler != null)
        {
            input.Set(characterInputHandler.GetNetworkInput());
        }
    }

    /// <summary>
    /// Handles actions when session is shutdown
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="shutdownReason"></param>
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("OnShhutDown");
    }

    /// <summary>
    /// Handles actions when server is disconnected
    /// </summary>
    /// <param name="runner"></param>
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("OnDisconnectedFromServer");
    }

    /// <summary>
    /// Handles actions when a request to connect is made
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="request"></param>
    /// <param name="token"></param>
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("OnConnectRequest");
    }

    /// <summary>
    /// Handles actions when a connection is failed
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="remoteAddress"></param>
    /// <param name="reason"></param>
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("OnConnectFailed");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    /// <summary>
    /// Handles actions when the session list is updated
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="sessionList"></param>
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        if (sessionListUIHandler == null)
            return;

        if (sessionList.Count == 0)
        {
            Debug.Log("Joined lobby no sessions found");

            sessionListUIHandler.OnNoSessionsFound();
        }
        else {
            sessionListUIHandler.ClearList();

            foreach (SessionInfo sessionInfo in sessionList)
            {
                sessionListUIHandler.AddToList(sessionInfo);
                Debug.Log($"Found session {sessionInfo.Name} playerCount {sessionInfo.PlayerCount}");
            }

        }
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    /// <summary>
    /// Handles actions on host migration
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="hostMigrationToken"></param>
    public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("OnHostMigration");

        await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

        FindObjectOfType<NetworkRunnerHandler>().StartHostMigration(hostMigrationToken);
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    /// <summary>
    /// Handles actions on host migration cleanup
    /// </summary>
    public void OnHostMigrationCleanUp() {
        Debug.Log("Spawner OnHostMigrationCleanUp started");

        foreach (KeyValuePair<int, NetworkPlayer> entry in mapTokenIDWithNetworkPlayer) {
            NetworkObject networkObjectInDictionary = entry.Value.GetComponent<NetworkObject>();

            if(networkObjectInDictionary.InputAuthority.IsNone) {
                Debug.Log($"{Time.time} Found player that has not reconnected. Despawning {entry.Value.nickName}");
                networkObjectInDictionary.Runner.Despawn(networkObjectInDictionary);
            }
        }

        Debug.Log("Spawner OnHostMigrationCleanUp completed");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }
}
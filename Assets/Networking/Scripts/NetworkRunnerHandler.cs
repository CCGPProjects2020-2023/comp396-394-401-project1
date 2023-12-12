/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Manages scenes on the network
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube

using System.Collections;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;

public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefab;
    NetworkRunner networkRunner;

    /// <summary>
    /// Awake method called by unity - Initializes properties.
    /// </summary>
    private void Awake()
    {
        NetworkRunner networkRunnerInScene = FindObjectOfType<NetworkRunner>();

        if (networkRunnerInScene != null)
            networkRunner = networkRunnerInScene;
    }

    /// <summary>
    /// Start method called by unity - Initializes properties.
    /// </summary>
    void Start()
    {
        if (networkRunner == null)
        {
            networkRunner = Instantiate(networkRunnerPrefab);
            networkRunner.name = "Network runner";

            if (SceneManager.GetActiveScene().name != "MultiplayerMenu") {
                var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, "TestSession", GameManager.instance.GetConnectionToken(), NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
            }            

            Debug.Log("Server NetworkRunner Started...");
        }        
    }

    /// <summary>
    /// Starts the normal, host migration.
    /// </summary>
    /// <param name="hostMigrationToken"></param>
    public void StartHostMigration(HostMigrationToken hostMigrationToken) {
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network runner - Migrated";

        var clientTask = InitializeNetworkRunnerHostMigration(networkRunner, hostMigrationToken);

        Debug.Log($"Host migration started");
    }


    /// <summary>
    /// Accesses the network scene manager.
    /// </summary>
    /// <param name="runner"></param>
    /// <returns></returns>
    INetworkSceneManager GetSceneManager(NetworkRunner runner) {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        return sceneManager;
    }

    /// <summary>
    /// Initializes and start the NetworkRunner
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="gameMode"></param>
    /// <param name="sessionName"></param>
    /// <param name="connectionToken"></param>
    /// <param name="address"></param>
    /// <param name="scene"></param>
    /// <param name="initialized"></param>
    /// <returns></returns>
    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionName, byte[] connectionToken, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized) {

        var sceneManager = GetSceneManager(runner);

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = sessionName,
            CustomLobbyName = "OurLobbyID",
            Initialized = initialized,
            SceneManager = sceneManager,
            ConnectionToken = connectionToken
        });
    }

    /// <summary>
    /// Initalizes the Host Migration system so that when the host disconnects, the game can go on
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="hostMigrationToken"></param>
    /// <returns></returns>
    protected virtual Task InitializeNetworkRunnerHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

        var sceneManager = GetSceneManager(runner);

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            SceneManager = sceneManager,
            HostMigrationToken = hostMigrationToken,
            HostMigrationResume = HostMigrationResume,
            ConnectionToken = GameManager.instance.GetConnectionToken()
        });
    }

    /// <summary>
    /// Resumes the host migration once the host is disconnected
    /// </summary>
    /// <param name="runner"></param>
    void HostMigrationResume(NetworkRunner runner) {
        Debug.Log($"HostMigrationResume started");

        foreach(var resumeNetworkObject in runner.GetResumeSnapshotNetworkObjects())
        {
            if (resumeNetworkObject.TryGetBehaviour<NetworkCharacterControllerPrototypeCustom>(out var characterController)) {
                runner.Spawn(resumeNetworkObject, position: characterController.ReadPosition(), rotation: characterController.ReadRotation(), onBeforeSpawned: (runner, newNetworkObject) => {
                    newNetworkObject.CopyStateFrom(resumeNetworkObject);

                    if(resumeNetworkObject.TryGetBehaviour<HPHandler>(out HPHandler oldHPHandler))
                    {
                        HPHandler newHPHandler = newNetworkObject.GetComponent<HPHandler>();
                        newHPHandler.CopyStateFrom(oldHPHandler);

                        newHPHandler.skipSettingStartValues = true;
                    }

                    if (resumeNetworkObject.TryGetBehaviour<NetworkPlayer>(out var oldNetworkPlayer)) {
                        FindObjectOfType<Spawner>().SetconnectionTokenMapping(oldNetworkPlayer.token, newNetworkObject.GetComponent<NetworkPlayer>());
                    }
                }); 
            }
        }

        StartCoroutine(CleanUpHostMigrationCO());

        Debug.Log($"HostMigrationResume completed");
    }

    /// <summary>
    /// Cleans up after host migration
    /// </summary>
    /// <returns></returns>
    IEnumerator CleanUpHostMigrationCO() {
        yield return new WaitForSeconds(5);

        FindObjectOfType<Spawner>().OnHostMigrationCleanUp();
    }

    /// <summary>
    /// Calls the JoinLobby method
    /// </summary>
    public void OnJoinLobby() {
        var clientTask = JoinLobby();
    }


    /// <summary>
    /// Handles actions when the player joins a lobby
    /// </summary>
    /// <returns></returns>
    private async Task JoinLobby() {
        Debug.Log("JoinLobby started");

        string lobbyID = "OurLobbyID";

        var result = await networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyID);

        if (!result.Ok)
        {
            Debug.LogError($"Unable to join lobby {lobbyID}");
        }
        else {
            Debug.Log("JoinLobby ok");
        }
    }

    /// <summary>
    /// Creates a game using the InitializeNetworkRunner method
    /// </summary>
    /// <param name="sessionName"></param>
    /// <param name="sceneName"></param>
    public void CreateGame(string sessionName, string sceneName) {
        Debug.Log($"Create session {sessionName} scene {sceneName} build index {SceneUtility.GetBuildIndexByScenePath($"scenes/{sceneName}")}");

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Host, sessionName, GameManager.instance.GetConnectionToken(), NetAddress.Any(), SceneUtility.GetBuildIndexByScenePath($"Networking/Scene/{sceneName}"), null);
    }

    /// <summary>
    /// Add a player to a session
    /// </summary>
    /// <param name="session"></param>
    public void JoinGame(SessionInfo session) {
        Debug.Log($"Join session {session.Name}");

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Client, session.Name, GameManager.instance.GetConnectionToken(), NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
    }
}
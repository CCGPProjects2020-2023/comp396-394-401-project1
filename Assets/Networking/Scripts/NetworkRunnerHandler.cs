using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        NetworkRunner networkRunnerInScene = FindObjectOfType<NetworkRunner>();

        if (networkRunnerInScene != null)
            networkRunner = networkRunnerInScene;
    }

    // Start is called before the first frame update
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

    public void StartHostMigration(HostMigrationToken hostMigrationToken) {
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network runner - Migrated";

        var clientTask = InitializeNetworkRunnerHostMigration(networkRunner, hostMigrationToken);

        Debug.Log($"Host migration started");
    }



    INetworkSceneManager GetSceneManager(NetworkRunner runner) {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        return sceneManager;
    }

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

    protected virtual Task InitializeNetworkRunnerHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

        var sceneManager = GetSceneManager(runner);

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            //GameMode = gameMode,
            //Address = address,
            //Scene = scene,
            //SessionName = "TestRoom",
            //Initialized = initialized,
            SceneManager = sceneManager,
            HostMigrationToken = hostMigrationToken,
            HostMigrationResume = HostMigrationResume,
            ConnectionToken = GameManager.instance.GetConnectionToken()
        });
    }

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

    IEnumerator CleanUpHostMigrationCO() {
        yield return new WaitForSeconds(5);

        FindObjectOfType<Spawner>().OnHostMigrationCleanUp();
    }

    public void OnJoinLobby() {
        var clientTask = JoinLobby();
    }

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

    public void CreateGame(string sessionName, string sceneName) {
        Debug.Log($"Create session {sessionName} scene {sceneName} build index {SceneUtility.GetBuildIndexByScenePath($"scenes/{sceneName}")}");

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Host, sessionName, GameManager.instance.GetConnectionToken(), NetAddress.Any(), SceneUtility.GetBuildIndexByScenePath($"Networking/Scene/{sceneName}"), null);
    }

    public void JoinGame(SessionInfo session) {
        Debug.Log($"Join session {session.Name}");

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Client, session.Name, GameManager.instance.GetConnectionToken(), NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
    }
}

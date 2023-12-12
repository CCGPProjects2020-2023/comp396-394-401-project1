/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Player on the network
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using UnityEngine;
using Fusion;
using TMPro;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public TextMeshProUGUI playerNickNameTM;
    public static NetworkPlayer Local { get; set; }
    public Transform playerModel;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName { get; set; }

    bool isPublicJoinMessageSent = false;

    public LocalCameraHandler localCameraHandler;
    public GameObject localUI;

    NetworkInGameMessages networkInGameMessages;

    [Networked] public int token { get; set; }

    /// <summary>
    /// Awake method called unity - Initializes properties.
    /// </summary>
    void Awake() {
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }

    /// <summary>
    /// Handles properties associated to the local player and ensures to disable other properties 
    /// such as the local camera if this is not the local player.
    /// </summary>
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            NetworkUtils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));

            if(Camera.main != null) Camera.main.gameObject.SetActive(false);

            AudioListener audioListener = GetComponentInChildren<AudioListener>(true);
            audioListener.enabled = true;

            localCameraHandler.localCamera.enabled = true;
            localCameraHandler.transform.parent = null;
            localUI.SetActive(true);

            RPC_SetNickName(GameManager.instance.playerNickName);

            Debug.Log("Spawned Local player");
        }
        else {
            localCameraHandler.localCamera.enabled = false;

            localUI.SetActive(false);

            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;

            localUI.SetActive(false);

            Debug.Log("Spawned remote player");
        }

        Runner.SetPlayerObject(Object.InputAuthority, Object);

        transform.name = $"P_{Object.Id}";
    }

    /// <summary>
    /// Handles the player when he or she leaves the room
    /// </summary>
    /// <param name="player"></param>
    public void PlayerLeft(PlayerRef player)
    {
        if (Object.HasStateAuthority) {
            if (Runner.TryGetPlayerObject(player, out NetworkObject playerLeftNetworkObject)) {
                if (playerLeftNetworkObject == Object)
                    Local.GetComponent<NetworkInGameMessages>().SendInGameRPCMessage(playerLeftNetworkObject.GetComponent<NetworkPlayer>().nickName.ToString(), "left");
            }            
        }
        
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    /// <summary>
    /// Calls proper method when the nickname is updated
    /// </summary>
    /// <param name="changed"></param>
    static void OnNickNameChanged(Changed<NetworkPlayer> changed) {
        Debug.Log($"{Time.time} OnNickNameChanged value {changed.Behaviour.nickName}");

        changed.Behaviour.OnNickNameChanged();
    }

    /// <summary>
    /// Updates the UI with the set nickname
    /// </summary>
    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");
        playerNickNameTM.text = nickName.ToString();
    }

    /// <summary>
    /// Sends the nickname to the network
    /// </summary>
    /// <param name="nickName"></param>
    /// <param name="ingo"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string nickName, RpcInfo ingo = default) {
        Debug.Log($"[RPC] SetNickName {nickName}");

        this.nickName = nickName;

        if(!isPublicJoinMessageSent)
        {
            networkInGameMessages.SendInGameRPCMessage(nickName, "joined");
            isPublicJoinMessageSent = true;
        }
    }

    /// <summary>
    /// Handles the destruction of the local camera
    /// </summary>
    void OnDestroy() {
        if (localCameraHandler != null)
            Destroy(localCameraHandler.gameObject);
    }
}

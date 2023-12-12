/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles player's health
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class HPHandler : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnHPChanged))]
    byte HP { get; set; }

    [Networked(OnChanged = nameof(OnStateChanged))]
    public bool isDead { get; set; }

    bool isInitialized = false;

    const byte startingHP = 5;

    public Color uiOnhitColor;
    public Image uiOnhitImage;

    public bool skipSettingStartValues = false;

    public SkinnedMeshRenderer bodyMeshRenderer;
    Color defaultMeshBodyColor;

    public GameObject playerModel;
    public GameObject deathGameObjectPrefab;

    HitboxRoot hitboxRoot;

    CharaterMovementHandler characteraterMovementHandler;
    NetworkInGameMessages networkInGameMessages;
    NetworkPlayer networkPlayer;

    /// <summary>
    /// Awake method called by unity - Initializes properties
    /// </summary>
    private void Awake()
    {
        characteraterMovementHandler = GetComponent<CharaterMovementHandler>();
        hitboxRoot = GetComponentInChildren<HitboxRoot>();
        networkPlayer = GetComponent<NetworkPlayer>();
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }

    /// <summary>
    /// Start method called by unity - initializes properties
    /// </summary>
    void Start()
    {
        if (!skipSettingStartValues) {
            HP = startingHP;
            isDead = false;
        }
       
        defaultMeshBodyColor = bodyMeshRenderer.material.color;

        isInitialized = true;
    }

    /// <summary>
    /// Changes the mesh renderes color to white for 0.2 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator OnHitCO() {

        bodyMeshRenderer.material.color = Color.white;

        if (Object.HasInputAuthority)
            uiOnhitImage.color = uiOnhitColor;

        yield return new WaitForSeconds(0.2f);

        bodyMeshRenderer.material.color = defaultMeshBodyColor;

        if(Object.HasInputAuthority && !isDead)
            uiOnhitImage.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// Respawns the character after 2 secs.
    /// </summary>
    /// <returns></returns>
    IEnumerator ServerReviveCO() {
        yield return new WaitForSeconds(2.0f);

        characteraterMovementHandler.RequestRespawn();
    }

    /// <summary>
    /// Diminishes the player health when it takes damage. 
    /// Sends a message to the RPCMessage when a player kills another player.
    /// </summary>
    /// <param name="damageCausedByPlayer"></param>
    /// <param name="damageAmount"></param>
    public void OnTakeDamage(string damageCausedByPlayer, byte damageAmount) { 
        if(isDead) return;

        if(damageAmount > HP) damageAmount = HP;

        HP -= damageAmount;

        Debug.Log($"{Time.time} {transform.name} took damage got {HP} left");

        if (HP <= 0) {
            networkInGameMessages.SendInGameRPCMessage(damageCausedByPlayer, $"Killed <b>{networkPlayer.nickName.ToString()}</b>");
            Debug.Log($"{Time.time} {transform.name} died");

            StartCoroutine( ServerReviveCO());

            isDead = true;
        }
    }

    /// <summary>
    /// Calls the OnHPReduced method based on appropriate condition
    /// </summary>
    /// <param name="changed"></param>
    static void OnHPChanged(Changed<HPHandler> changed) {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.HP}");   

        byte newHP = changed.Behaviour.HP;

        changed.LoadOld();

        byte oldHP = changed.Behaviour.HP;

        if(newHP < oldHP) changed.Behaviour.OnHPReduced();
    }

    /// <summary>
    /// Starts the coroutine when called
    /// </summary>
    private void OnHPReduced() { 
        if(!isInitialized) { return; }

        StartCoroutine(OnHitCO());
    }

    /// <summary>
    /// Checks if player is dead, and handles witha appropriate behavior.
    /// </summary>
    /// <param name="changed"></param>
    static void OnStateChanged(Changed<HPHandler> changed) {
        Debug.Log($"{Time.time} OnStateChanged value {changed.Behaviour.isDead}");

        bool isDeadCurrent = changed.Behaviour.isDead;

        changed.LoadOld();

        bool oldIsDead = changed.Behaviour.isDead;

        if(isDeadCurrent) changed.Behaviour.OnDeath();
        else if(!isDeadCurrent && oldIsDead)
            changed.Behaviour.OnRevive();
    }

    /// <summary>
    /// Handles actions when player is dead.
    /// </summary>
    private void OnDeath()
    {
        Debug.Log($"{Time.time} OnDeath");

        playerModel.gameObject.SetActive(false);
        hitboxRoot.HitboxRootActive = false;
        characteraterMovementHandler.SetCharacterControllerEnabled(false);    
        
        Instantiate(deathGameObjectPrefab, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Revive the player by enabling the model, hitbox, and movement handler.
    /// </summary>
    private void OnRevive()
    {
        Debug.Log($"{Time.time} OnRevive");

        if (Object.HasInputAuthority) { 
            uiOnhitImage.color = new Color(0,0,0,0);    
        }

        playerModel.gameObject.SetActive(true);
        hitboxRoot.HitboxRootActive = true;
        characteraterMovementHandler.SetCharacterControllerEnabled(true);
    }

    /// <summary>
    /// Sets appropriate properties when respawned.
    /// </summary>
    public void OnRespawned() {
        HP = startingHP;
        isDead = false;
    }
}
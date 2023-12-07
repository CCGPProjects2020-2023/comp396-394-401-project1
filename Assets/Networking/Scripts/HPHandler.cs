using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        characteraterMovementHandler = GetComponent<CharaterMovementHandler>();
        hitboxRoot = GetComponentInChildren<HitboxRoot>();
        networkPlayer = GetComponent<NetworkPlayer>();
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!skipSettingStartValues) {
            HP = startingHP;
            isDead = false;
        }
       
        defaultMeshBodyColor = bodyMeshRenderer.material.color;

        isInitialized = true;
    }

    IEnumerator OnHitCO() {

        bodyMeshRenderer.material.color = Color.white;

        if (Object.HasInputAuthority)
            uiOnhitImage.color = uiOnhitColor;

        yield return new WaitForSeconds(0.2f);

        bodyMeshRenderer.material.color = defaultMeshBodyColor;

        if(Object.HasInputAuthority && !isDead)
            uiOnhitImage.color = new Color(0, 0, 0, 0);
    }

    IEnumerator ServerReviveCO() {
        yield return new WaitForSeconds(2.0f);

        characteraterMovementHandler.RequestRespawn();
    }

    public void OnTakeDamage(string damageCausedByPlayer) { 
        if(isDead) return;

        HP -= 1;

        Debug.Log($"{Time.time} {transform.name} took damage got {HP} left");

        if (HP <= 0) {
            networkInGameMessages.SendInGameRPCMessage(damageCausedByPlayer, $"Killed <b>{networkPlayer.nickName.ToString()}</b>");
            Debug.Log($"{Time.time} {transform.name} died");

            StartCoroutine( ServerReviveCO());

            isDead = true;
        }
    }
    static void OnHPChanged(Changed<HPHandler> changed) {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.HP}");   

        byte newHP = changed.Behaviour.HP;

        changed.LoadOld();

        byte oldHP = changed.Behaviour.HP;

        if(newHP < oldHP) changed.Behaviour.OnHPReduced();
    }

    private void OnHPReduced() { 
        if(!isInitialized) { return; }

        StartCoroutine(OnHitCO());
    }

    static void OnStateChanged(Changed<HPHandler> changed) {
        Debug.Log($"{Time.time} OnStateChanged value {changed.Behaviour.isDead}");

        bool isDeadCurrent = changed.Behaviour.isDead;

        changed.LoadOld();

        bool oldIsDead = changed.Behaviour.isDead;

        if(isDeadCurrent) changed.Behaviour.OnDeath();
        else if(!isDeadCurrent && oldIsDead)
            changed.Behaviour.OnRevive();
    }

    private void OnDeath()
    {
        Debug.Log($"{Time.time} OnDeath");

        playerModel.gameObject.SetActive(false);
        hitboxRoot.HitboxRootActive = false;
        characteraterMovementHandler.SetCharacterControllerEnabled(false);    
        
        Instantiate(deathGameObjectPrefab, transform.position, Quaternion.identity);
    }

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

    public void OnRespawned() {
        HP = startingHP;
        isDead = false;
    }
}
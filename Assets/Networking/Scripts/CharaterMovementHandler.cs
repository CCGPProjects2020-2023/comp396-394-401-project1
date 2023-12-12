using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharaterMovementHandler : NetworkBehaviour
{
    public Animator characterAnimator;
    
    bool isRespawnRequested = false;

    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    HPHandler hPHandler;
    NetworkInGameMessages networkInGameMessages;
    NetworkPlayer networkPlayer;

    float walkSpeed = 0;


    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        hPHandler = GetComponent<HPHandler>();
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
        networkPlayer = GetComponent<NetworkPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority) {
            if(isRespawnRequested)
            {
                Respawn();
                return;
            }

            if (hPHandler.isDead) return;
        }
        

        if (GetInput(out NetworkInputData networkInputData))
        {

            transform.forward = networkInputData.aimForwardVector;

            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;

            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            if (networkInputData.isJumpPressed)
                networkCharacterControllerPrototypeCustom.Jump();

            Vector2 walkVector = new Vector2(networkCharacterControllerPrototypeCustom.Velocity.x, networkCharacterControllerPrototypeCustom.Velocity.z);
            walkVector.Normalize();

            walkSpeed = Mathf.Lerp(walkSpeed, Mathf.Clamp01(walkVector.magnitude), Runner.DeltaTime * 5);
            characterAnimator.SetFloat("walkSpeed", walkSpeed);

            characterAnimator.SetBool("isShooting", networkInputData.isFirePressed);

            Debug.Log("IsFiring Input: " + networkInputData.isFirePressed);
            

            CheckFallRespawn();
        }
    }  

    void CheckFallRespawn() {
        if (transform.position.y < -12) {
            if(Object.HasStateAuthority)
            {
                Debug.Log($"{Time.time} Respawn due to fall outside of map at position {transform.position}");

                networkInGameMessages.SendInGameRPCMessage(networkPlayer.nickName.ToString(), "fell off the world");

                Respawn();
            }
        }
    }

    public void SetCharacterControllerEnabled(bool isEnabled) { 
        networkCharacterControllerPrototypeCustom.Controller.enabled = isEnabled;
    }

    public void RequestRespawn() {
        isRespawnRequested = true;
    }

    void Respawn() { 
        networkCharacterControllerPrototypeCustom.TeleportToPosition(NetworkUtils.GetRandomSpawnPoint());

        hPHandler.OnRespawned();

        isRespawnRequested = false;
    }
}

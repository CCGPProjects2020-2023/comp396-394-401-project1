/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles network character movement
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
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

    /// <summary>
    /// Awake method called by unity - Initializes properties
    /// </summary>
    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        hPHandler = GetComponent<HPHandler>();
        networkInGameMessages = GetComponent<NetworkInGameMessages>();
        networkPlayer = GetComponent<NetworkPlayer>();
    }

    /// <summary>
    /// Method similar to the update() method from unity, but on the network.
    /// Used to check the status and inputs of the player.
    /// </summary>
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

            if (networkInputData.isJumpPressed) networkCharacterControllerPrototypeCustom.Jump();

            Vector2 walkVector = new Vector2(networkCharacterControllerPrototypeCustom.Velocity.x, networkCharacterControllerPrototypeCustom.Velocity.z);
            walkVector.Normalize();

            walkSpeed = Mathf.Lerp(walkSpeed, Mathf.Clamp01(walkVector.magnitude), Runner.DeltaTime * 5);
            characterAnimator.SetFloat("walkSpeed", walkSpeed);

            characterAnimator.SetBool("isShooting", networkInputData.isFirePressed);

            Debug.Log("IsFiring Input: " + networkInputData.isFirePressed);
            
            CheckFallRespawn();
        }
    }  

    /// <summary>
    /// Checks if the player has fallen off the world.
    /// </summary>
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

    /// <summary>
    /// Enables or disables the character controller.
    /// </summary>
    /// <param name="isEnabled">Sets the controller to this</param>
    public void SetCharacterControllerEnabled(bool isEnabled) { 
        networkCharacterControllerPrototypeCustom.Controller.enabled = isEnabled;
    }

    /// <summary>
    /// Sets the property isRespawnRequested to true.
    /// </summary>
    public void RequestRespawn() {
        isRespawnRequested = true;
    }

    /// <summary>
    /// Respawns the character in the world and sets the isRespawnRequested property to false.
    /// </summary>
    void Respawn() { 
        networkCharacterControllerPrototypeCustom.TeleportToPosition(NetworkUtils.GetRandomSpawnPoint());

        hPHandler.OnRespawned();

        isRespawnRequested = false;
    }
}
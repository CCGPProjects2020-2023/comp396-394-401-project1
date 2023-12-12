/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles network player inputs
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    bool isFireButtonPressed = false;
    bool isProjectileButtonPressed = false;

    LocalCameraHandler localCameraHandler;
    CharaterMovementHandler characterMovementHandler;
    
    /// <summary>
    /// Awake method called by unity - Initializes properties
    /// </summary>
    private void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponent<CharaterMovementHandler>();
    }

    /// <summary>
    /// Start method called by unity - Locks the cursor on the screen
    /// </summary>
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;       
    }

    /// <summary>
    /// Update method called by unity - Ensures that the player who has who has contorl also has authority
    /// </summary>
    void Update()
    {
        if (!characterMovementHandler.Object.HasInputAuthority) return;

        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1;

        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Jump")) isJumpButtonPressed = true;

        if (Input.GetButtonDown("Fire1")) isFireButtonPressed = true;

        if(Input.GetKeyDown(KeyCode.G)) isProjectileButtonPressed = true;

        localCameraHandler.SetViewInputVector(viewInputVector);
    }

    /// <summary>
    /// Gets the input from the NetworkInputData object
    /// </summary>
    /// <returns>NetworkInputData object</returns>
    public NetworkInputData GetNetworkInput() { 
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        networkInputData.movementInput = moveInputVector;

        networkInputData.isJumpPressed = isJumpButtonPressed;

        networkInputData.isFirePressed = isFireButtonPressed;

        networkInputData.isProjectilePressed = isProjectileButtonPressed;

        isJumpButtonPressed = false;
        isFireButtonPressed = false;
        isProjectileButtonPressed=false;

        return networkInputData;
    }
}
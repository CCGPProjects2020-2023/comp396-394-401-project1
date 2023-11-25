/*
    //***NOTE: This code is modified from the COMP396 classwork examples***

    Author's Name: Alexander  Maynard
    Creation Date: October 23, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: November 25, 2023
    Program Description: This is script manages the PlayerAbilities using the StateMachine script implementation. 
    Furthermore, the states managed in this file are AbilitiesReady, Phase, Teleport and Cooldown.
    There are state checks, handlers for each state and respective states that are called depending on what conditions are true and what input is pressed.
    
    Revision History:
    -October 23, 2023 
        -> Created the initial boilerplate/template for the playerAbilities that has states for the abilities. 
        -> Added the initail variables, playeStateMachine instance in start, factory pattern for the onEnter, onFrame and onExit for the phase and teleport states. 
        -> Added Handlers and method calls for phase and teleport. 
        -> Added methods for Shoot, MovePlayer and Jump Methods called from the update as they should be separate from the states of the abilites.

    -October 24, 2023
        -> Added comment headers, funciton summaries and inline comments for all code in the PlayController.cs file.
        -> Implemented simple version of movement code.
        -> Added the initail variables, playeStateMachine instance in start, factory pattern for the onEnter, onFrame and onExit for the new cooldown and abilitiesReady states. 
        -> Added handlers and empty methods for cooldown and abilitiesReady states.
        -> Empty Jump() method was removed for now.
        -> Added cooldown length and currentTime variabels as well as cooldownTimer method and cooldownDone method evaluate if cooldown is done.
        -> Added simple debug code for each onFrame call and methods called  from their respective onFrame call.
        -> Added code to check the transition between abilititesReady and phase/teleport.
        -> Added abilitesReadyCheck variable(bool)
        -> Added calls to cooldownOnFrame from phase and teleport.
        -> Added functionality to the cooldownOnFrame, Cooldown and cooldownDone methods.
    
    -October 26, 2023
        -> Moved all player movement and playerShoot contents from this script to the playerController script and renamed this script to PlayerAbilities
    -November 3, 2023
        -> Added functionality for the phase ability
        -> Organized code better
        -> Added more/better comments for the whole file
        ->Disabled teleport for now
    -November 10, 2023
        -> Tied Phase indicator UI element to the player phase ability and added relevant comments\
        -> Tied AbilitiesStatusText element to the PlayerAbilities script to indicate what is going on to the player. 
        -> Added relevant comments to the change above.
    -November 24, 2023
        -> Uncommented code for teleport and finished it's implementation here
        ->Tied the teleport implementation to the UI (Ui images and text)
    -November 25, 2023
        -> Refactored OnPhase, OnTeleport and comments.
        -> Also added initial comments to the cooldown method.
 */

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

/// <summary>
/// This is the PLayerAbilities state machine implementation. 
/// It controls the flow and implementation of the player abilities phase 
/// and teleport (at a later date) abilities, as well as any relevant transition states.
/// </summary>
public class PlayerAbilities : MonoBehaviour
{
    [Header("General Player Ability Attributes")]
    //this denotes the currentTime since phase 
    [SerializeField] private float timeSinceAbilityUsed = 0.0f;
    //time to cooldown after ability is done
    public float cooldownAfterAbilities = 3.0f;
    //checks if abilities can be used again
    public bool abilitiesReadyCheck = true;

    [Header("Phase Specific Attributes")]
    //duration of the player phase ability
    public float phaseDuration = 3.0f;
    //to check if phase is active
    public bool canPhase = false;
    public GameObject phaseIndicator; //image denoting phasing

    [Header("Teleport Specific Attributes")]
    //to check if phase is active
    public bool teleported = false;
    public GameObject teleportIndicator; //image denoting teleportation

    [Header("Abilities Status Text element")]
    public TextMeshProUGUI abilitiesStatusText;

    [Header("Player Camera Reference")]
    public Camera playerCam;

    //player states declared **NOTE: teleport state is disabled for now
    private StateMachine playerAbilitesStateMachine;
    private StateMachine.State abilitiesReady, phase, teleport, cooldown;


    //Start creates the inistances of the State Machine and player states
    // Start is called before the first frame update
    void Start()
    {
        //new instance of StateMachine
        playerAbilitesStateMachine = new StateMachine();

        //Use factory pattern
        //abilitiesReady state onEnter, onExit and onFrame calls
        abilitiesReady = playerAbilitesStateMachine.CreateState("AbilitiesReady");
        abilitiesReady.onEnter = delegate { Debug.Log("AbilitiesReady.onEnter"); };
        abilitiesReady.onExit = delegate { Debug.Log("AbilitiesReady.onExit"); };
        abilitiesReady.onFrame = AbilitiesReadyOnFrame;

        //phase state onEnter, onExit and onFrame calls
        phase = playerAbilitesStateMachine.CreateState("Phase");
        phase.onEnter = delegate { Debug.Log("Phase.onEnter"); };
        phase.onExit = delegate { Debug.Log("Phase.onExit"); };
        phase.onFrame = PhaseOnFrame;

        //teleport state onEnter, onExit and onFrame calls
        teleport = playerAbilitesStateMachine.CreateState("Teleport");
        teleport.onEnter = delegate { Debug.Log("Teleport.onEnter"); };
        teleport.onExit = delegate { Debug.Log("Teleport.onExit"); };
        teleport.onFrame = TeleportOnFrame;

        //cooldown state onEnter, onExit and onFrame calls
        cooldown = playerAbilitesStateMachine.CreateState("Cooldown");
        cooldown.onEnter = delegate { Debug.Log("Cooldown.onEnter"); };
        cooldown.onExit = delegate { Debug.Log("Cooldown.onExit"); };
        cooldown.onFrame = CooldownOnFrame;
    }


    /// <summary>
    /// Update calls the playerStateMachine.Update() every frame.
    /// Also Updates the timer since phase
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //playerAbilitesStateMachine.Update is called from this update instance
        playerAbilitesStateMachine.Update();
        //timer for cooldown time (amount of time before the player can use the next ability) code here:
        timeSinceAbilityUsed += 1 * Time.deltaTime;
    }

    /// <summary>
    /// This is the OnFrame for the abilities. 
    /// It doesn't do anything except for wait for an ability to get used. Then it will call the proper State Change.
    /// </summary>
    private void AbilitiesReadyOnFrame()
    {
        //text to display to the user for the status of the abilites
        abilitiesStatusText.text = "Abilties Status: Ready";

        //transitions to other states
        if (Input.GetKeyDown(KeyCode.E))
            playerAbilitesStateMachine.ChangeState(phase);
        
        if (Input.GetKeyDown(KeyCode.F))
            playerAbilitesStateMachine.ChangeState(teleport);
    }

    /// <summary>
    /// This is the Cooldown OnFrame and handles the Cooldown after an ability is used.
    /// </summary>
    private void CooldownOnFrame()
    {
        Debug.Log("Cooldown.onFrame");
        //call to cooldown functionality
        Cooldown();

        //if the cooldown time is complete call the abilitesReady state
        if (abilitiesReadyCheck == true)
            playerAbilitesStateMachine.ChangeState(abilitiesReady);
    }

    /// <summary>
    /// This is the OnFrame for the phase ability. This calls the functionality for phase and 
    /// then sets the abilitiesReadyCheck to false and timer to 0 again so no other abilities can be used until the cooldown is complete
    /// </summary>
    void PhaseOnFrame()
    {
        Debug.Log("Phase.onFrame");
        //Call phase(happens once before the exit condition is called)
        Phase();

        
        //if canPhase is true (phase is active) it calls the cooldown state as the cooldown state handles the ability ending.
        if(canPhase == true)
            playerAbilitesStateMachine.ChangeState(cooldown);
    }

    /// <summary>
    /// Phase method (functionality)
    /// </summary>
    private void Phase()
    {
        //text to display to the user for the status of the abilites
        abilitiesStatusText.text = "Abilties Status: Phase Active";

        Debug.Log("Phase used");

        //Reset currentCooldownTime and set abilitiesReadyCheck to false, transitions to other state can only go to coolown after phase is done once automatically.
        abilitiesReadyCheck = false;
        timeSinceAbilityUsed = 0;
        canPhase = true;
        
        //sets the phase indicator as active (or visible)
        phaseIndicator.SetActive(true);

        //sets the Player and Phaseable layers as not being able to interact with each other or 'phase'
        Physics.IgnoreLayerCollision(7, 6, true);

        //sets the Player and canDamage(from enemy ammo) layers as not being able to interact with each other or 'phase'
        Physics.IgnoreLayerCollision(7, 10, true);
    }


    /// <summary>
    /// OnFrame for the Hunt state
    /// </summary>
    void TeleportOnFrame()
    {
        Debug.Log("Teleport.onFrame");
        
        // Call teleport(happens once before the exit condition is called)
        Teleport();

        //if teleported is true (player teleported) it calls the cooldown state as the cooldown state handles the ability ending.
        if (teleported == true)
            playerAbilitesStateMachine.ChangeState(cooldown);
    }

    /// <summary>
    /// Teleport method (functionality). Here a ray will be cast to teleport the player where the player points the cursor.
    /// The abilityStatus text, abilitiesReady and teleported checks will be set properly and the timeSinceAbilityUsed will be reset to 0
    /// </summary>
    private void Teleport()
    {
        //text to display to the user for the status of the abilites
        abilitiesStatusText.text = "Abilties Status: Teleported!";

        Debug.Log("Player just teleported");

        //teleportation action here from ray
        //point to be teleported to 
        RaycastHit teleportPoint;

        //ray to be cast from the center of the screen (where the mouse is)
        Ray teleportRay = playerCam.ScreenPointToRay(Input.mousePosition);


        //set the teleport point where the ray hits a collider
        if (Physics.Raycast(teleportRay, out teleportPoint))
        {
            //set transform of the player to the teleport point
            this.transform.position = teleportPoint.point;//objectHit.position;
        }

        //sets the teleport indicator as active (or visible)
        teleportIndicator.SetActive(true);

        //Reset currentCooldownTime and set abilitiesReadyCheck to false, transitions to other state can only go to coolown after teleport is done once automatically.
        abilitiesReadyCheck = false;
        timeSinceAbilityUsed = 0;
        //checks teleport to true to denote that we teleported
        teleported = true;
    }


    //timer to be used for both ability cooldowns and phase ending
    //cooldown state handles the abilities ending and decommissioning the abitiy changes to their previous states 
    //depending on the timer.
    private void Cooldown()
    {
        Debug.Log("Ability is cooling down");

        //checks if the time of the ability used is greater than half the time for abilities cooldown and that ateleport was used (teleport == true).
        //this is needed as we need time to display "Abilties Status: Teleported!" from the teleport implementation and the "Abilties Status: Cooling down..." in the cooldown implementation.
        //also sets the indicator for teleportation as invisible again
        if (timeSinceAbilityUsed >= cooldownAfterAbilities/2 && teleported == true)
        {
            //sets the teleport indicator as inactive (or invisible)
            teleportIndicator.SetActive(false);

            //text to display to the user for the status of the abilites
            abilitiesStatusText.text = "Abilties Status: Cooling down...";
        }

        //checks if the time of the ability used is greater than the time for abilites cooldown and that teleport was used (teleport == true).
        //if so we set the abilitesReadyCheck to true (to transition states) and set teleported back to false
        if (timeSinceAbilityUsed >= cooldownAfterAbilities && teleported == true)
        {
            //sets the ability check to true to denote that the player ability cooldown is over. This is necessary for the change in state from cooldown to abilitiesReady
            abilitiesReadyCheck = true;
            teleported = false;
        }

        //checks if the timeSince an ability was used is > or = to the phase duration set
        if (timeSinceAbilityUsed >= phaseDuration && teleported == false)
        {
            //text to display to the user for the status of the abilites
            abilitiesStatusText.text = "Abilties Status: Cooling down...";
            //sets the phase indicator as inactive (or not visible) after phase is over
            phaseIndicator.SetActive(false);

            //If true the player and 'Phaseable' layer (for certain walls and such) not ignore each other anymore.
            Physics.IgnoreLayerCollision(7, 6, false);

            //sets the Player and canDamage(from enemy ammo) layers as not being able to interact with each other or 'phase'
            Physics.IgnoreLayerCollision(7, 10, false);

            //can no longer phase
            canPhase = false;
        }


        //checks if the time since an ability was used is greater than the cooldownAfterAbilities + phaseDuration.
        //It also checks whether teleport or phase activated by the 'teleported' bool.
        //This denotes that full duration of the phase has ability occured and that there is buffer time for the cooldown afterward
        if ((timeSinceAbilityUsed >= cooldownAfterAbilities + phaseDuration) && teleported == false)
        {
            //set this to false to reflect that the player can't pahse anymore in the editor.
            canPhase = false;
            //sets the ability check to true to denote that the player ability cooldown is over. This is necessary for the change in state from cooldown to abilitiesReady
            abilitiesReadyCheck = true;
        }
    }
}

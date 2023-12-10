/*
 * //---------------------------------------------------------------------------
 * //*** NOTE: THIS CODE IS MODIFIED FROM THE COMP396 CLASSWORK LAB EXAMPLES ***
 * //---------------------------------------------------------------------------
 * 
 * 
 * Author's Name:           Alexander  Maynard
 * Creation Date:           October 23, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 10, 2023
 * 
 * Program Description:     This is script manages the PlayerAbilities using the StateMachine script implementation. 
 *                          Furthermore, the states managed in this file are AbilitiesReady, Phase, Teleport and Cooldown.
 *                          There are state checks, handlers for each state and respective states that are called depending 
 *                          on what conditions are true and what ability input is pressed by the player.
 * 
 * Revision History:        October 23, 2023: 
 *                          -> Created the initial boilerplate/template for the playerAbilities that has states for the abilities. 
 *                          -> Added the initail variables, playeStateMachine instance in start, factory pattern for the onEnter, onFrame and onExit for the phase and teleport states. 
 *                          -> Added Handlers and method calls for phase and teleport. 
 *                          -> Added methods for Shoot, MovePlayer and Jump Methods called from the update as they should be separate from the states of the abilites.
 *                          
 *                          October 24, 2023:
 *                          -> Added comment headers, funciton summaries and inline comments for all code in the PlayController.cs file.
 *                          -> Implemented simple version of movement code.
 *                          -> Added the initail variables, playeStateMachine instance in start, factory pattern for the onEnter, onFrame and onExit for the new cooldown and abilitiesReady states. 
 *                          -> Added handlers and empty methods for cooldown and abilitiesReady states.
 *                          -> Empty Jump() method was removed for now.
 *                          -> Added cooldown length and currentTime variabels as well as cooldownTimer method and cooldownDone method evaluate if cooldown is done.
 *                          -> Added simple debug code for each onFrame call and methods called  from their respective onFrame call.
 *                          -> Added code to check the transition between abilititesReady and phase/teleport.
 *                          -> Added abilitesReadyCheck variable(bool)
 *                          -> Added calls to cooldownOnFrame from phase and teleport.
 *                          -> Added functionality to the cooldownOnFrame, Cooldown and cooldownDone methods.
 *                          
 *                          October 26, 2023:
 *                          -> Moved all player movement and playerShoot contents from this script to the playerController script and renamed this script to PlayerAbilities
 *                          
 *                          November 3, 2023:
 *                          -> Added functionality for the phase ability
 *                          -> Organized code better
 *                          -> Added more/better comments for the whole file
 *                          ->Disabled teleport for now
 *                          
 *                          November 10, 2023:
 *                          -> Tied Phase indicator UI element to the player phase ability and added relevant comments\
 *                          -> Tied AbilitiesStatusText element to the PlayerAbilities script to indicate what is going on to the player. 
 *                          -> Added relevant comments to the change above.
 *                          
 *                          November 24, 2023:
 *                          -> Uncommented code for teleport and finished it's implementation here
 *                          -> Tied the teleport implementation to the UI (Ui images and text)
 *                          
 *                          November 25, 2023:
 *                          -> Refactored OnPhase, OnTeleport and comments.
 *                          -> Also added initial comments to the cooldown method.
 *                          -> Added sounds for the player abilities in script and added sounds (done with minimal refactoring).
 *                          
 *                          November 28, 2023:
 *                          -> Added instantiation for particles when player teleports or phases
 *                          
 *                          December 1, 2023:
 *                          -> Commented out all Debug.Log() -> they are no longer needed.   
 *                          
 *                          December 3, 2023:
 *                              -> Changed public variables to private, updated comments/comments headers, removed unecessary usings and  tidied the script up.
 *                              -> Commented out Debug.Logs and update some variable names.
 *                          December 7, 2023:
 *                              -> Added layerMask and slightly refactored raycast for teleporation to only hit the "Default" or "Ground" layers.
 *                          
 *                          December 10, 2023:
 *                              -> Refactored code to include a check if the pause menu is active. This will play into whether the abilities work or not. 
 */

using TMPro;
using UnityEngine;


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
    [SerializeField] private float cooldownAfterAbilities = 3.0f;
    //checks if abilities can be used again
    [SerializeField] private bool abilitiesReadyCheck = true;

    [Header("Player Particles Reference")]
    [SerializeField] private GameObject abilityParticles;

    [Header("Phase Specific Attributes")]
    //duration of the player phase ability
    [SerializeField] private float phaseDuration = 3.0f;
    //to check if phase is active
    [SerializeField] private bool isPhasing = false;
    [SerializeField] private GameObject phaseIndicator; //image denoting phasing

    [Header("Teleport Specific Attributes")]
    //to check if phase is active
    [SerializeField] private bool teleported = false;
    [SerializeField] private GameObject teleportIndicator; //image denoting teleportation

    [Header("Abilities Status Text element")]
    [SerializeField] private TextMeshProUGUI abilitiesStatusText; //text on player UI tell the player important information.

    [Header("Player Camera Reference")]
    [SerializeField] private Camera playerCam; // refernece to the player camera


    [Header("InGamePauseMenu Object")]
    //to return isPaused from InGamePauseMenu.cs
    [SerializeField] private InGamePauseMenu pauseMenu;


    //instance of player machine machine
    private StateMachine playerAbilitesStateMachine;
    //player states declared for the playerAbilites StateMachine
    private StateMachine.State abilitiesReady, phase, teleport, cooldown;


    /// <summary>
    /// Start creates the instances of each StateMachine PlayerAbility states (onEnter, onExit and onFrame).
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        //new instance of StateMachine for PlayerAbilities StateMachine
        playerAbilitesStateMachine = new StateMachine();

        //Use factory pattern

        //abilitiesReady state onEnter, onExit and onFrame calls
        abilitiesReady = playerAbilitesStateMachine.CreateState("AbilitiesReady");
        abilitiesReady.onEnter = delegate { }; //Debug.Log("AbilitiesReady.onEnter");
        abilitiesReady.onExit = delegate { }; //Debug.Log("AbilitiesReady.onExit");
        abilitiesReady.onFrame = AbilitiesReadyOnFrame;

        //phase state onEnter, onExit and onFrame calls
        phase = playerAbilitesStateMachine.CreateState("Phase");
        phase.onEnter = delegate { }; //Debug.Log("Phase.onEnter");
        phase.onExit = delegate { }; //Debug.Log("Phase.onExit");
        phase.onFrame = PhaseOnFrame;

        //teleport state onEnter, onExit and onFrame calls
        teleport = playerAbilitesStateMachine.CreateState("Teleport");
        teleport.onEnter = delegate { }; //Debug.Log("Teleport.onEnter");
        teleport.onExit = delegate { }; //Debug.Log("Teleport.onExit");
        teleport.onFrame = TeleportOnFrame;

        //cooldown state onEnter, onExit and onFrame calls
        cooldown = playerAbilitesStateMachine.CreateState("Cooldown");
        cooldown.onEnter = delegate { }; //Debug.Log("Cooldown.onEnter");
        cooldown.onExit = delegate { }; //Debug.Log("Cooldown.onExit");
        cooldown.onFrame = CooldownOnFrame;
    }


    /// <summary>
    /// Update checks if pauseMenu is active or not. 
    /// If the puaseMenu is active then just return.
    /// If the pauseMenu is false then Update calls the playerStateMachine.Update() every frame.
    /// Also Updates the timer since an ability has been used.
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //inverted if. If pauseMenu == true just return as we dont want the camera to move. Otherwise call MoveCamera();
        if (pauseMenu.isPaused == true) return;
        //playerAbilitesStateMachine.Update is called from this update instance
        playerAbilitesStateMachine.Update();
        //timer for cooldown time (amount of time before the player can use the next ability) code here:
        timeSinceAbilityUsed += 1 * Time.deltaTime;
    }

    /// <summary>
    /// This is the OnFrame for the abilities. 
    /// It doesn't do anything except for wait for an ability to get used with press of 'E' or 'F' keys,
    /// then it will call the proper State Change.
    /// </summary>
    private void AbilitiesReadyOnFrame()
    {
        //text to display to the user for the status of the abilites
        abilitiesStatusText.text = "Abilties Status: Ready";

        //*** No method for abilitesReadyOnFrame to call as we are only waiting for user input ***

        //transitions to other states
        if (Input.GetKeyDown(KeyCode.E))
            playerAbilitesStateMachine.ChangeState(phase);
        
        if (Input.GetKeyDown(KeyCode.F))
            playerAbilitesStateMachine.ChangeState(teleport);
    }

    /// <summary>
    /// This is the OnFrame for the phase ability. This calls the functionality for phase and 
    /// checks is canPhase == true to change to cooldown state
    /// </summary>
    void PhaseOnFrame()
    {
        //Debug.Log("Phase.onFrame");
        //Call phase(happens once before the exit condition is called)
        Phase();

        
        //if isPhasing is true (phase is active) it calls the cooldown state as the cooldown state handles the ability ending.
        if(isPhasing == true)
            playerAbilitesStateMachine.ChangeState(cooldown);
    }

    /// <summary>
    /// Phase state functionality is called here:
    /// -particles, sounds effects, UI indicators and layer interactions for phase
    /// are all set here.
    /// </summary>
    private void Phase()
    {
        //text to display to the user for the status of the abilites
        abilitiesStatusText.text = "Abilties Status: Phase Active";

        //instantiate particles once player teleports
        Instantiate(abilityParticles, this.transform.position, this.transform.rotation);

        //Debug.Log("Phase used");

        //Set abilitiesReadyCheck to false as abilites will no longer be ready to be used
        abilitiesReadyCheck = false;

        //Reset timeSinceAbilityUsed
        timeSinceAbilityUsed = 0;

        //player is now phasing
        isPhasing = true;
        
        //sets the phase indicator as active (or visible) --> for the player UI
        phaseIndicator.SetActive(true);

        //sets the Player and Phaseable layers as not being able to interact with each other or 'phase'
        Physics.IgnoreLayerCollision(7, 6, true);

        //sets the Player and canDamage(from enemy ammo) layers as not being able to interact with each other or 'phase'
        Physics.IgnoreLayerCollision(7, 10, true);

        //phase sound from the SoundManager script
        SoundManager.Instance.PlaySfx(SfxEvent.Phase);
    }

    /// <summary>
    /// This is the Teleport OnFrame and handles the when the teleport ability.
    /// </summary>
    void TeleportOnFrame()
    {
        //Debug.Log("Teleport.onFrame");
        
        // Call teleport functionality
        Teleport();

        //if teleported == true then player teleported, so call the cooldown state
        if (teleported == true)
            playerAbilitesStateMachine.ChangeState(cooldown);
    }

    /// <summary>
    /// Teleport state functionality. Here a ray will be cast to teleport the player where the player points the cursor.
    /// The abilityStatus text, abilitiesReady and teleported checks will be set properly and the timeSinceAbilityUsed will be reset to 0.
    /// </summary>
    private void Teleport()
    {
        //text to display to the user for the status of the abilites --> on the player UI
        abilitiesStatusText.text = "Abilties Status: Teleported!";

        //Debug.Log("Player just teleported");


        //bit wise shift for layerMask. It is used so the raycast should only hit "Default" or "Ground" layer
        int layerMask = (1 << 0) | (1 << 3);


        //ray to be cast from the center of the screen (where the mouse or reticle is)
        Ray teleportRay = playerCam.ScreenPointToRay(Input.mousePosition);


        //set the teleport point where the ray hits a collider on layer "Default"
        if (Physics.Raycast(teleportRay, out RaycastHit teleportPoint, Mathf.Infinity, layerMask))
        {
            //set transform of the player to the teleport point
            this.transform.position = teleportPoint.point; //objectHit.position;
            
            //instantiate ability particles once player teleports
            Instantiate(abilityParticles, this.transform.position, this.transform.rotation);
        }

        //sets the teleport indicator as active (or visible) --> on player UI
        teleportIndicator.SetActive(true);
       
        //Set abilitiesReadyCheck to false as abilites will no longer be ready to be used
        abilitiesReadyCheck = false;

        //Reset timeSinceAbilityUsed
        timeSinceAbilityUsed = 0;

        //checks teleport to true to denote that we teleported
        teleported = true;

        //teleport sound from the SoundManager script
        SoundManager.Instance.PlaySfx(SfxEvent.Teleport);
    }

    /// <summary>
    /// This is the Cooldown OnFrame and handles the Cooldown after an ability is used.
    /// </summary>
    private void CooldownOnFrame()
    {
        //Debug.Log("Cooldown.onFrame");
        //call to cooldown functionality
        Cooldown();

        //if abilitiesReadyCheck == true (based on abilities cooldown() functionality)... then change to abilitesReady state
        if (abilitiesReadyCheck == true)
            playerAbilitesStateMachine.ChangeState(abilitiesReady);
    }

    /// <summary>
    /// Cooldown state functionality. Here cooldown state handles 
    /// the abilities ending and decommissioning the abitiy changes to their previous state 
    /// and then making abilitiesREacdyCheck == true for CooldownOnFrame to call the abilitiesReady state.
    /// </summary>
    private void Cooldown()
    {
        //Debug.Log("Ability is cooling down");

        //***------CHECKS FOR TELEPORT BEGIN------***

        //checks if the time of the ability used is greater than half the time for abilities cooldown and that teleport was used.
        //this is needed as we need time to display "Abilties Status: Teleported!" from the teleport implementation and the "Abilties Status: Cooling down..." in the cooldown implementation.
        //also sets the indicator for 'teleportated' as invisible again
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
        
        //***------CHECKS FOR TELEPORT END ------***



        //***------CHECK FOR PHASE COOLDOWN BEGIN------***

        //checks if the timeSince an ability was used is >= to the phase duration set and that isPhasing == true 
        //so that we can determine that phase was used.
        if (timeSinceAbilityUsed >= phaseDuration && isPhasing == true)
        {
            //text to display to the user for the status of the abilites
            abilitiesStatusText.text = "Abilties Status: Cooling down...";
            //sets the phase indicator as inactive (or not visible) after phase is over
            phaseIndicator.SetActive(false);

            //If true the player and 'Phaseable' layer (for certain walls and such) not ignore each other anymore.
            Physics.IgnoreLayerCollision(7, 6, false);

            //sets the Player and canDamage(from enemy ammo) layers as not being able to interact with each other or 'phase'
            Physics.IgnoreLayerCollision(7, 10, false);

            //no longer phasing
            isPhasing = false;

            //phase sound from the SoundManager script
            SoundManager.Instance.PlaySfx(SfxEvent.Phase);

            //instantiate particles once player teleports
            Instantiate(abilityParticles, this.transform.position, this.transform.rotation);
        }

        //checks if the time since an ability was used is greater than the cooldownAfterAbilities + phaseDuration.
        //It also checks whether teleport or phase activated by the 'teleported' bool (isPhasing was already set to false).
        //This denotes that full duration of the phase has ability occured and that there is buffer time for the cooldown afterward
        if ((timeSinceAbilityUsed >= cooldownAfterAbilities + phaseDuration) && teleported == false)
        {
            //set this to false to reflect that the player isn't phasing anymore in the editor.
            isPhasing = false;
            //sets the ability check to true to denote that the player ability cooldown is over. This is necessary for the change in state from cooldown to abilitiesReady
            abilitiesReadyCheck = true;
        }

        //***------CHECK FOR PHASE COOLDOWN END------***
    }
}

/*
    //***NOTE: This code is modified from the COMP396 classwork examples***

    Author's Name: Alexander  Maynard
    Creation Date: October 23, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: November 3, 2023
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
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//make documentation for every class and function (just description. What does this function/class)
/// <summary>
/// 
/// </summary>
public class PlayerAbilities : MonoBehaviour
{

    [Header("General Player Ability Attributes")]
    //this denotes the currentTime since phase 
    [SerializeField] private float timeSinceAbilityUsed = 0.0f;
    //time to cooldown after ability is done
    public float cooldownAfterABilities = 3.0f;
    //checks if abilities can be used again
    public bool abilitiesReadyCheck = true;



    [Header("Phase Specific Attributes")]
    //duration of the player phase ability
    public float phaseDuration = 3.0f;
    //to check if phase is active
    public bool canPhase = false;


    //player states declared **NOTE: teleport state is disabled for now
    private StateMachine playerAbilitesStateMachine;
    private StateMachine.State abilitiesReady, phase, cooldown; //teleport not here for now



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


        //**Not active for now

        ////teleport state onEnter, onExit and onFrame calls
        //teleport = playerAbilitesStateMachine.CreateState("Teleport");
        //teleport.onEnter = delegate { Debug.Log("Teleport.onEnter"); };
        //teleport.onExit = delegate { Debug.Log("Teleport.onExit"); };
        //teleport.onFrame = TeleportOnFrame;

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
        //transitions to other states
        if (Input.GetKeyDown(KeyCode.E))
            playerAbilitesStateMachine.ChangeState(phase);
        
        //**NOTE: Disabled for now
        //if (Input.GetKeyDown(KeyCode.F))
            //playerAbilitesStateMachine.ChangeState(teleport);
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

        
        //instantly calls the cooldown state as the cooldown state handles the ability ending.
        playerAbilitesStateMachine.ChangeState(cooldown);
    }

    /// <summary>
    /// Phase method (functionality)
    /// </summary>
    private void Phase()
    {
        Debug.Log("Phase used");

        //Reset currentCooldownTime and set abilitiesReadyCheck to false, transitions to other state can only go to coolown after phase is done once automatically.
        abilitiesReadyCheck = false;
        timeSinceAbilityUsed = 0;
        canPhase = true;


        Physics.IgnoreLayerCollision(7, 6, true);
    }





    //**ALL TELEPORT CODE IS NOT ACTIVE FOR NOW

    ///// <summary>
    ///// OnFrame for the Hunt state
    ///// </summary>
    //void TeleportOnFrame()
    //{
    //    //Debug.Log("Teleport.onFrame");
    //    // Call teleport(happens once before the exit condition is called)
    //    Teleport();

    //    //instantly calls the cooldown state as the cooldown state handles the ability ending.
    //    playerAbilitesStateMachine.ChangeState(cooldown);
    //}

    ///// <summary>
    ///// Teleport method (functionality). NO FUNCTIONALITY FOR NOW
    ///// </summary>
    //private void Teleport()
    //{
    //    Debug.Log("Player just teleported");

    //    //Reset currentCooldownTime and set abilitiesReadyCheck to false, transitions to other state can only go to coolown after teleport is done once automatically.
    //    abilitiesReadyCheck = false;
    //    timeSinceAbilityUsed = 0;
    //}


    //timer to be used for both ability cooldowns and phase ending
    private void Cooldown()
    {
        Debug.Log("Ability is cooling down");


        //checks if the timeSince an ability was used is > or = to the phase duration set. 
        if(timeSinceAbilityUsed >= phaseDuration)
        {
            //If true the player and 'Phaseable' layer (for certain walls and such) not ingnore each other anymore.
            Physics.IgnoreLayerCollision(7, 6, false);
            canPhase = false;
        }


        //checks if the time since an ability was used is greater than the cooldownAfterAbilities + phaseDuration.
        //This denotes that full duration of the phase has ability occured and that there is buffer time for the cooldown afterward
        if ((timeSinceAbilityUsed >= cooldownAfterABilities + phaseDuration))
        {
            //set this to false to reflect that the player can't pahse anymore in the editor.
            canPhase = false;
            //sets the ability check to true to denote that the player ability cooldown is over. This is necessary for the change in state from cooldown to abilitiesReady
            abilitiesReadyCheck = true;
        }
    }
}

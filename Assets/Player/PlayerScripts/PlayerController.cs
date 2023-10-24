/*
    //***NOTE: This code is modified from the COMP396 classwork examples***

    Author's Name: Alexander  Maynard
    Creation Date: October 23, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: October 24, 2023
    Program Description: This is the simple player controller using state machine implementation and movement code. 
    There are state checks, handlers for each state and respective states that are called depenmding on what conditions are true.
    
    Revision History:
    -October 23, 2023 
        -> Created the initial boilerplate/template for the playerController that has states for the abilities. 
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
        ->Added functionality to the cooldownOnFrame, Cooldown and cooldownDone methods.
 */

using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.XR;


//make documentation for every class and function (just description. What does this function/class)
/// <summary>
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    //By Alexander Maynard (301170707) for the COMP396-394 group project
    //NOTE: This code is referenced from the COMP396 classwork examples
    
    
    //player variables -> not part of the player ability state states
    public Rigidbody player;
    public float health = 100;
    public float speed = 16;
    public float cooldownLength = 3.0f;
    public float currentCooldownTime = 0.0f;
    public bool abilitiesReadyCheck = true;


    //player states declared
    private PlayerStateMachine playerAbilitesStateMachine;
    private PlayerStateMachine.State abilitiesReady, phase, teleport, cooldown;

    // Start is called before the first frame update
    void Start()
    {
        //new instance of StateMachine
        playerAbilitesStateMachine = new PlayerStateMachine();

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
    /// Update also well as the plaeyr controls for movement, jumping and shooting.
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //playerAbilitesStateMachine.Update is called from this update instance
        playerAbilitesStateMachine.Update();

        //this code is not controller by the playerStateMachine. It is player derived purely from player inputs so it is independant 
        //should have some code for attacking and running here. Other abilites like phase and telport should be in states.
        MovePlayer();

        //if key is pressed call the shoot method.
        if(Input.GetKey(KeyCode.Mouse0))
            Shoot();
        //timer code here:
        currentCooldownTime += 1 * Time.deltaTime;
    }

    /// <summary>
    /// 
    /// </summary>
    private void AbilitiesReadyOnFrame()
    {
        //transitions to other states
        if(Input.GetKeyDown(KeyCode.E))
            playerAbilitesStateMachine.ChangeState(phase);
        if(Input.GetKeyDown(KeyCode.F))
            playerAbilitesStateMachine.ChangeState(teleport);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void CooldownOnFrame()
    {
        Debug.Log("Cooldown.onFrame");
        Cooldown();
        if (abilitiesReadyCheck == true)
            playerAbilitesStateMachine.ChangeState(abilitiesReady);
    }

    /// <summary>
    /// 
    /// </summary>
    void PhaseOnFrame()
    {
        Debug.Log("Roam.onFrame");
        //Call phase(happens once before the exit condition is called)
        Phase();

        //Reset currentCooldownTime and set abilitiesReadyCheck to false, transitions to other state can only go to coolown after phase is done once automatically.
        abilitiesReadyCheck = false;
        currentCooldownTime = 0;
        playerAbilitesStateMachine.ChangeState(cooldown);
    }

    /// <summary>
    /// Phase method (functionality)
    /// </summary>
    private void Phase()
    {
        Debug.Log("Just phased");
    }

    /// <summary>
    /// OnFrame for the Hunt state
    /// </summary>
    void TeleportOnFrame()
    {
        Debug.Log("Teleport.onFrame");
        // Call teleport(happens once before the exit condition is called)
        Teleport();


        //Reset currentCooldownTime and set abilitiesReadyCheck to false, transitions to other state can only go to coolown after teleport is done once automatically.
        abilitiesReadyCheck = false;
        currentCooldownTime = 0;
        playerAbilitesStateMachine.ChangeState(cooldown);
    }

    /// <summary>
    /// Teleport method (functionality)
    /// </summary>
    private void Teleport()
    {
        Debug.Log("Player just teleported");
    }

    /// <summary>
    /// Method that calls the shooting code if button is pressed
    /// </summary>
    private void Shoot()
    {
        //shooting code here -> for now just Debug.Log message.
        Debug.Log("Player is shooting");
    }


    /// <summary>
    /// Method that calls the MovePlayer code. This code takes player Horizontal and Vertical input and translates that
    /// to player movement using the player Rigidbody and the speed variable.
    /// </summary>
    private void MovePlayer()
    {
        //Taking horizontal and vertical input 
        Vector3 playerMovement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        //Take input and use it to move the player in the world
        player.velocity = new Vector3((playerMovement.x * speed * 1000 * Time.deltaTime), player.velocity.y, (playerMovement.y * speed * 1000 * Time.deltaTime));
    }



    //timer to be used for ability cooldown
    private void Cooldown()
    {
        Debug.Log("Ability is cooling down");
        if (currentCooldownTime >= cooldownLength)
            abilitiesReadyCheck = true;
    }
}
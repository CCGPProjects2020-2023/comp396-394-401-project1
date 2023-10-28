/*
    //***NOTE: This code is modified from the COMP396 classwork examples***

    Author's Name: Alexander  Maynard
    Creation Date: October 26, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: October 26, 2023
    Program Description: This is the simple playerController that handles player movement and shooting as well as any other player controls
    
    Revision History: 
    -October 26, 2023
        -> Created initial playerController with only update that contains a call to MovePlayer and Shoot methods
        -> Addedplayer movement code that uses player inout to move the player
        -> Added links to references used to help learn how to make the input always know where the front and right are based on rotation.
        -> Added Debug.Log to shoot method to test that it was being called 
        -> Added small logic fix to GetAxises to always know where forward and right are based on the player rotation.
        -> Added more comments
        
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
/// This class controls the player movement and shooting.
/// </summary>
public class PlayerController : MonoBehaviour
{
    //reference to the player and player speed.
    public Rigidbody player;
    public float speed = 16;


    void Update()
    {
        //this code is not controller by the playerStateMachine. It is player derived purely from player inputs so it is independant 
        //should have some code for attacking and running here. Other abilites like phase and telport should be in states.
        MovePlayer();

        //if key is pressed call the shoot method.
        if (Input.GetKey(KeyCode.Mouse0))
            Shoot();
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
        //used this to learn how to get forward position of the object for movement in relation to the object rotation:
        // https://stackoverflow.com/questions/62140867/get-wasd-keys-to-follow-camera-rotation-in-unity

        //set playerMovement to 0, Vector3.zero is shorthand for 0
        Vector3 playerMovement = Vector3.zero;


        //**These inputs increment the direction of the player movement in parallel to the player rotation**

        //if the horizontal movement is in the positive direction, increment the playerMovement equal the player's red rotation axis (right)
        if (Input.GetAxis("Horizontal") > 0.1) 
            playerMovement += transform.right;
        //if the horizontal movement is in the negative direction, increment the playerMovement equal the opposite of the player's red rotation axis (right)
        if (Input.GetAxis("Horizontal") < -0.1)
            playerMovement -= transform.right;
        //if the vertical movement is in the positive direction, increment the playerMovement equal the player's blue rotation axis (forward)
        if (Input.GetAxis("Vertical") > 0.1)
            playerMovement += transform.forward;
        //if the vertical movement is in the negative direction, increment the playerMovement equal the opposite of the player's blue rotation axis (forward)
        if (Input.GetAxis("Vertical") < -0.1)
            playerMovement -= transform.forward;

        //Take input and use it to move the player in the world
        player.velocity = new Vector3((playerMovement.x * speed * 1000 * Time.deltaTime), player.velocity.y, (playerMovement.z * speed * 1000 * Time.deltaTime));
    }
}
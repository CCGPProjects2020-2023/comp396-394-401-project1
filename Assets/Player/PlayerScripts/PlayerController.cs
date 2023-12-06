/**
    //***NOTE: This code is modified from the COMP396 classwork examples***

    Author's Name: Alexander  Maynard
    Creation Date: October 26, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: November 25, 2023
    Program Description: This is the simple playerController that handles player movement and shooting as well as any other player controls
    
    Revision History: 
    -October 26, 2023
        -> Created initial playerController with only update that contains a call to MovePlayer and Shoot methods
        -> Addedplayer movement code that uses player inout to move the player
        -> Added links to references used to help learn how to make the input always know where the front and right are based on rotation.
        -> Added Debug.Log to shoot method to test that it was being called 
        -> Added small logic fix to GetAxises to always know where forward and right are based on the player rotation.
        -> Added more comments  
    -October 30, 2023
        ->Added code//implementation for Jumping.
        ->Added some comments
        ->Refactored movement code
    -November 01, 2023
        -> Updated the jump from GetKeyUp to GetKey
        -> Fixed error in if(!Grounded logic for the player jump)
        -> udpated case values from 0.01 and -0.01 to 0.1 and -0.1 for player switch case movement in the player horizontal and vertical player movement
    -November 03, 2023
        ->Added player health and comments
        ->Started simple player shooting
    -November 03, 2023
        -> Added shooting delay to the script
        -> Included more comments 
    -November 10, 2023
        -> Fixed shooting not from center bug
        -> Refactored code and added headers
        -> Added a reference to a score manager object.
    -November 25, 2023
        -> Added player sounds and comments.
        -> Changed jumping OnCollision type for the jump
 */

using System;
using UnityEngine;


//make documentation for every class and function (just description. What does this function/class)
/// <summary>
/// This class controls the player movement, jumping and shooting.
/// </summary>
public class PlayerController : MonoBehaviour
{
    

    //Player variables
    //[Header("General Player Attributes")]
    //player variables -> not part of the player ability state states
    //[SerializeField] private float health = 100;

    //reference to the player and player speed.
    [Header("General Player Attributes")]
    public float speed = 16;
    //Jumping code variables
    public float jumpPower = 8.0f;
    public bool isGrounded = false;

    [Header("Shooting Attributes")]
    //Delay for shooting variables
    public float shootingDelay = 0.5f;
    public float currentTime = 0.0f;

    //Player Riogidbody reference
    //[Header("Reference to player Rigidbody")]
    private Rigidbody player;

    [Header("GameObject References")]
    //bullet object
    public GameObject bullet;
    //camera object
    public GameObject playerCamera;
    //score manager object
    public GameObject scoreManager;

    private void Start()
    {
        player = this.gameObject.GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        //this code is not controller by the playerStateMachine. It is player derived purely from player inputs so it is independant 
        //should have some code for attacking and running here. Other abilites like phase and telport should be in states.
        MovePlayer();

        //if key is pressed call the shoot method.
        if (Input.GetKey(KeyCode.Mouse0) && currentTime >= shootingDelay)
            Shoot();

        //isGrounded = GroundCheck();

        if (isGrounded && Input.GetKey(KeyCode.Space))
            Jump();

        //timer for shooting delay
        currentTime += 1 * Time.deltaTime;
    }

    /// <summary>
    /// Method that calls the shooting code if button is pressed
    /// </summary>
    private void Shoot()
    {
        //NOTE: This ressource was used as a research point to figure out how to get center of the screen: https://forum.unity.com/threads/how-to-get-a-world-position-from-the-center-of-the-screen.524573/

        //Gets the middle of the screen rather than the mouse position
        Vector3 cameraShootPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 0));

        //shooting code here -> for now just Debug.Log message.
        Debug.Log("Player is shooting at: " + cameraShootPoint + "and rotation: " + playerCamera.transform.position);
        //Instantiates the bullet prefab where the player camera x=0, y=0, z=0 is and with it's rotation 
        Instantiate(bullet, cameraShootPoint, playerCamera.transform.rotation);
        currentTime = 0;

        //shooting sound from the SoundManager script
        SoundManager.Instance.PlaySfx(SfxEvent.GunShot);
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
       
        //Switch statement for the Horizontal movement (only one horizontal axis direction can be true at once)
        switch (Input.GetAxis("Horizontal")) 
        {

            //if the horizontal movement is in the positive direction, increment the playerMovement equal the player's red rotation axis (right)
            case > 0.1f:
                playerMovement += transform.right;
                break;
            //if the horizontal movement is in the negative direction, increment the playerMovement equal the opposite of the player's red rotation axis (right)
            case < -0.1f:
                playerMovement -= transform.right;
                break;
        }

        //Switch statement for the vertical movement (only one vertical axis direction can be true at once)
        switch (Input.GetAxis("Vertical"))
        {
            //if the vertical movement is in the positive direction, increment the playerMovement equal the player's blue rotation axis (forward)
            case > 0.1f:
                playerMovement += transform.forward;
                break;
            //if the vertical movement is in the negative direction, increment the playerMovement equal the opposite of the player's blue rotation axis (forward)
            case < -0.1f:
                playerMovement -= transform.forward;
                break;
        }

        //if the player is grounded, player can move normally
        if (isGrounded)
            //Take input and use it to move the player in the world
            player.velocity = new Vector3((playerMovement.x * speed * 1000 * Time.deltaTime), player.velocity.y, (playerMovement.z * speed * 1000 * Time.deltaTime));


        //if player is not grounded, player has reduced movement in the air
        if (!isGrounded)
            //Take input and use it to move the player in the world -> speed multiplier (i.e. speed * (1000)) is reduced to 2/10 or 200 for forward/backward and side to side movement while the player is in the air
            player.velocity = new Vector3((playerMovement.x * speed * 200 * Time.deltaTime), player.velocity.y, (playerMovement.z * speed * 200 * Time.deltaTime));
    }


    /// <summary>
    /// Method for Jump here. It just calls AddFore for the player on Impulse to send the player upward when called
    /// </summary>
    private void Jump()
    {
        player.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    /// <summary>
    /// Checks if player hit the ground after jump. If so then call the jump land sound
    /// </summary>
    /// <param name="other">References the other object in the collision -> the method checks if this ground</param>
    private void OnCollisionEnter(Collision other)
    {
        //layer == 3 is "Ground" layer
        //if player is touching a gameobject with tag == "Ground"
        if(other.gameObject.layer == 3)
        {
            //jump landing sound from the SoundManager script
            SoundManager.Instance.PlaySfx(SfxEvent.JumpLanding);
        }
    }

    /// <summary>
    /// This is called when the player collider is colliding with the grounds collider. If this method is called then the groundCheck is set to true
    /// </summary>
    /// <param name="other">References the other object in the collision -> the method checks if this ground</param>
    private void OnCollisionStay(Collision other)
    {
        //layer == 3 is "Ground" layer
        //if player is touching a gameobject with tag == "Ground"
        if (other.gameObject.layer == 3)
        {
            //set isGrouded to true
            isGrounded = true;
        }
    }


    /// <summary>
    /// This is called when the player collider leaves the ground collider. If this method is called then the groundCheck is set to false
    /// </summary>
    /// <param name="other">References the other object in the collision -> the method checks if this ground</param>
    private void OnCollisionExit(Collision other)
    {
        //layer == 3 is "Ground" layer
        //if player is not touching a gameobject with tag == "Ground"
        if (other.gameObject.layer == 3)
        {
            //set isGrouded to false
            isGrounded = false;
        }
    }
}
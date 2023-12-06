/*
 * Author's Name:           Alexander  Maynard
 * Creation Date:           October 26, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 3, 2023
 * 
 * Program Description:     This is the simple playerController that handles player movement 
 *                          and shooting as well as any other player controls
 * 
 * Revision History:        October 26, 2023:
 *                             -> Created initial playerController with only update that contains a call to MovePlayer and Shoot methods
 *                              -> Addedplayer movement code that uses player inout to move the player
 *                              -> Added links to references used to help learn how to make the input always know where the front and right are based on rotation.
 *                              -> Added Debug.Log to shoot method to test that it was being called 
 *                              -> Added small logic fix to GetAxises to always know where forward and right are based on the player rotation.
 *                              -> Added more comments  
 *                          
 *                          October 30, 2023:
 *                              -> Added code//implementation for Jumping.
 *                              -> Added some comments
 *                              -> Refactored movement code
 *                          
 *                          November 01, 2023:
 *                              -> Updated the jump from GetKeyUp to GetKey
 *                              -> Fixed error in if(!Grounded logic for the player jump)
 *                          
 *                          November 03, 2023:
 *                              -> Added player health and comments
 *                              -> Started simple player shooting
 *                          
 *                          November 03, 2023:
 *                              -> Added shooting delay to the script
 *                              -> Included more comments 
 *                          
 *                          November 10, 2023
 *                              -> Fixed shooting not from center bug
 *                              -> Refactored code and added headers
 *                              -> Added a reference to a score manager object.
 *                          
 *                          November 25, 2023:
 *                              -> Added player sounds and comments.
 *                              -> Changed jumping OnCollision type for the jump
 *                          
 *                          November 30, 2023:
 *                              -> Added anims and other changes to the script so that it fits the soldier asset purchased from the Unity Asset store. 
 *                          
 *                          December 1, 2023:
 *                              -> Continued work on anims and other changes to the script so that it fits the soldier asset purchased from the Unity Asset store.
 *                              -> Reworked the movement code for the anims to work better.
 *                          
 *                          December 2, 2023:
 *                              -> Refactored the shoot() code a change from instantiation to rays for shooting.
 *                              -> Delete unused code and refactored a bit.-> Tied muzzle flash particle system to shoot.
 *                          
 *                          December 3, 2023:
 *                              -> Removed unecessary usings, made variables private and updated comments, comment headers, variable names and refactored code.
*/

using UnityEngine;


/// <summary>
/// This class controls the player movement, jumping and shooting.
/// </summary>
public class PlayerController : MonoBehaviour
{
    //reference to the player variables
    [Header("General Player Attributes")]
    [SerializeField] private float speed = 16;
    //Jumping power
    [SerializeField] private float jumpPower = 8.0f;
    //check for if player is grounded
    [SerializeField] private bool isGrounded = false;

    [Header("Shooting Attributes")]
    //Delay for shooting
    [SerializeField] private float shootingDelay = 0.5f;
    //current time since shooting
    [SerializeField] private float shootingCurrentTime = 0.0f;

    [Header("GameObject References")]
    //Player Rigidbody reference
    [SerializeField] private Rigidbody player;
    //camera object
    [SerializeField] private Camera playerCamera; //used to be GamObject
    //score manager object
    [SerializeField] public GameObject scoreManager; //this has to be public for the target.cs to be able to call it.

    [Header("Player Animator")]
    //player animator
    [SerializeField] private Animator playerAnimator;

    [Header("Muzzle-Flash Particle System Variables")]
    //muzzle-flash particle system
    [SerializeField] private GameObject muzzleFlash;
    //bool to toggle between SetActive true & false for muzzle-flash particle system
    [SerializeField] private bool muzzleToggle = false; //starts false

    /// <summary>
    /// Start gets the player Rigidbody
    /// </summary>
    private void Start()
    {
        //et the player Rigidbody
        player = this.gameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// FixedUpdate calls all essential player functionality: Move, Shoot, Jump, 
    /// and increments the shootingCurrentTime (for shooting delay). FixedUpdate was used for consistency.
    /// </summary>
    void FixedUpdate()
    {
        //this code is not controller by the playerStateMachine. It is player derived purely from player inputs so it is independant 
        //should have some code for attacking and running here. Other abilites like phase and telport should be in states.
        MovePlayer();

        //if key is pressed call the shoot method
        if (Input.GetKey(KeyCode.Mouse0) && shootingCurrentTime >= shootingDelay)
            Shoot();

        //if isGrounded && jump button is pressed call jump
        if (isGrounded && Input.GetKey(KeyCode.Space))
            Jump();

        //timer for current time of the shooting delay
        shootingCurrentTime += 1 * Time.deltaTime;
    }

    /// <summary>
    /// Functionality for the player shooting. It essentially uses a raycast on only the
    /// "Enemy" layer to send message to damage the enemy. It aslo calls any animations
    /// or sounds for shooting.
    /// </summary>
    private void Shoot()
    {
        //bit wise shift for layerMask. It is used so the raycast should only hit "Enemy" layer
        int layerMask = 1 << 11;

        //reset the current time for the shooting delay
        shootingCurrentTime = 0;

        //calls muzzlFlashToggle to toggle the muzzle-flash particle system (aka turn it on)
        MuzzleFlashToggle();
        //call muzzleFlashToggle again after 0.10 seconds (aka turn it off)
        Invoke(nameof(MuzzleFlashToggle), 0.1f);

        //shooting sound from the SoundManager script
        SoundManager.Instance.PlaySfx(SfxEvent.GunShot);

        //Raycast only on the "Enemy" layer and then call the HitEnemy (through SendMessage()) function in Target.cs to damage the enemy and set the score.
        //if the shootPoint point from the ray hits a collider (from the mouse position) on layer "Enemy" then...
        if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit shootPoint, Mathf.Infinity, layerMask))
        {
            //Debug.Log("hit" + shootPoint.transform.gameObject);
            //... on that shootPoint send a Message to HitEnemy(in Target.cs for the enemy hit)
            shootPoint.collider.SendMessage("HitEnemy");
        }
    }

    /// <summary>
    /// This sets the muzzleFlash particle system (near the reticle on the player UI) as setActive(true).
    /// or setActive(false) to activate and deactivate it in the scene.
    /// </summary>
    public void MuzzleFlashToggle()
    {
        //bool muzzleToggle is changed to the opposite of before it was called (ex: false is now true)
        muzzleToggle = !muzzleToggle;
        //call set active with new value for muzzleToggle
        muzzleFlash.SetActive(muzzleToggle);
    }

    /// <summary>
    /// Method that calls the MovePlayer code. This code takes player Horizontal and Vertical input and translates that
    /// to player movement using the player Rigidbody and the speed variable.
    /// </summary>
    private void MovePlayer()
    {
        //used this to learn how to get forward position of the object for movement in relation to the object rotation:
        // https://stackoverflow.com/questions/62140867/get-wasd-keys-to-follow-camera-rotation-in-unity

        //set playerMovement to 0, Vector3.zero is shorthand for 0 -> this should happen every call to update to regualte the execution of playerMovement
        Vector3 playerMovement = Vector3.zero;

        //**These inputs increment the direction of the player movement in parallel to the player rotation** --> using .forward and .right
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            //if the horizontal movement is in the positive direction, increment the playerMovement equal the player's red rotation axis (right)
            playerMovement += transform.right;
        } 
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            //if the horizontal movement is in the negative direction, increment the playerMovement equal the opposite of the player's red rotation axis (right)
            playerMovement -= transform.right;
        }
        if (Input.GetAxis("Vertical") > 0.1f)
        {
            //if the vertical movement is in the positive direction, increment the playerMovement equal the player's blue rotation axis (forward)
            playerMovement += transform.forward;
        }
        if (Input.GetAxis("Vertical") < -0.1f)
        {
            //if the vertical movement is in the negative direction, increment the playerMovement equal the opposite of the player's blue rotation axis (forward)
            playerMovement -= transform.forward;
        }

        //for animations when to axis movement is true --> for example: right and forward are both true.
        if ((Input.GetAxis("Vertical") < 0.1f && (Input.GetAxis("Vertical") > -0.1f) || (Input.GetAxis("Horizontal") < 0.1f && (Input.GetAxis("Horizontal") > -0.1f))))
        {
            //set animations to no movement
            playerAnimator.SetBool("twoAxisMovement", false);
        }
        else
        {
            //set animations for corner cases (such as left & right or back & left movement at the same time for example) that there is movement (might not be used)
            playerAnimator.SetBool("twoAxisMovement", true);
        }

        //animator for float for transitioning between movement forward, backward, left and right
        //for the x axis
        playerAnimator.SetFloat("xVelocity", Input.GetAxis("Horizontal"));
        //for the z axis
        playerAnimator.SetFloat("zVelocity", Input.GetAxis("Vertical"));

        //Take input and use it to move the player in the world
        player.velocity = new Vector3((playerMovement.x * speed * 1000 * Time.deltaTime), player.velocity.y, (playerMovement.z * speed * 1000 * Time.deltaTime));
    }

    /// <summary>
    /// Method for Jump here. It just calls AddFore for the player on Impulse to send the player upward when called.
    /// Alse sets the triggers the player jump animation in the animator.
    /// </summary>
    private void Jump()
    {
        //call anim trigger for jump
        playerAnimator.SetTrigger("jumpPressed");
        player.AddForce(transform.up * jumpPower, ForceMode.Impulse);
    }

    /// <summary>
    /// Checks if player hit the ground after jump. If so then call the jump land sound
    /// and sets the animation bool for jump to false
    /// </summary>
    /// <param name="other">References the other object in the collision -> the method checks if this ground</param>
    private void OnCollisionEnter(Collision other)
    {
        //layer == 3 is "Ground" layer
        //if player is touching a gameobject with tag == "Ground"
        if(other.gameObject.layer == 3)
        {
            playerAnimator.SetBool("jumping", false);
            //jump landing sound from the SoundManager script
            SoundManager.Instance.PlaySfx(SfxEvent.JumpLanding);
        }
    }

    /// <summary>
    /// This is called when the player collider is colliding with the grounds collider. If this method is called then the groundCheck is set to true.
    /// It also sets the animation bool for jump to false
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
            playerAnimator.SetBool("jumping", false);
        }
    }

    /// <summary>
    /// This is called when the player collider leaves the ground collider. If this method is called then the groundCheck is set to false
    /// and the jumping animation bool is set to true.
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
            playerAnimator.SetBool("jumping", true);
        }
    }
}
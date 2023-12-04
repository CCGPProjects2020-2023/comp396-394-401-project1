/*
 * Author's Name:           Alexander Maynard
 * Creation Date:           November 11, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 3, 2023
 * 
 * Program Description:     This is a simple healthManager script that handles health, damage, damage immunity and etc 
 *                          for the player and calls the appropriate scene, sounds and animation changes whent the player is
 *                          hurt or dead.
 * 
 * Revision History:    November 11, 2023:
 *                          -> Added health variables and other object references.
 *                          -> Added functionality for the slider to decrease upon the referenced object getting damaged.
 *                          -> added player death and enemy death (empty) methods.
 *                      
 *                      November 25, 2023:
 *                          -> Added the Toggle_IsImmune and Add_Health methods.
 *                          -> Added player hit and player death sounds
 *                          -> Removed enemyDeath call method for now and refactored the health UI to only workk for player.
 *                          
 *                      December 1, 2023:
 *                          -> Added player animator to call the player death anim in the playerController.
 *                      
 *                      December 2, 2023:
 *                          -> Refactored code and removed uneeded code.
 *                      
 *                      December 3, 2023:
 *                          -> Changed public variables to private and updated comments/comments headers. 
 *                          -> Also refactored the OnTriggerEnter() method and removed unecessary usings.
 */


using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// This script manages player health, damage, damage immunity and etc 
/// It also calls the appropriate scene, sounds and animation changes whent the player is hurt or dead.
/// </summary>
public class HealthManager : MonoBehaviour
{
    [Header("Health Slider Reference")]
    //health slider object reference
    [SerializeField] private Slider healthSliderHandle;

    [Header("Health Attributes of referenced object")]
    //health value for object this scriIt is attached to.
    [SerializeField] private int health = 100;
    //starting health value
    protected internal int health_start = 0;
    //set the damage that the referenced object will take
    [SerializeField] private int damageToTake = 10;

    [Header("Is Player Immune to damage?")]
    [SerializeField] private bool is_immune = false;

    [Header("Slider to be disabled upon health at 0")]
    //slider object refrence to disable when health is at zero --> otherwise there is always a bit of health showing.
    [SerializeField] private GameObject healthAtZero;
    


    //---------------------------------------------------------------------
    //** These fields should not be accessible to the editor **
    //Player animator to call the player death animation later on
    private Animator playerAnimator;

    //references to all playerscripts (except for camera controller script)
    private PlayerController playerController;
    private UpdatePlayerRotation updatePlayerRotation;
    private PlayerAbilities PlayerAbilities;
    //---------------------------------------------------------------------


    /// <summary>
    /// Start gets all player related scripts (except for camera) on the player.
    /// It also assigns the health_start to health which is initially 100 and
    /// sets the healthSliderHandle.value == to health.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        //get reference to the player scripts to disable upon player death.
        playerController = GetComponent<PlayerController>();
        updatePlayerRotation = GetComponent<UpdatePlayerRotation>();
        PlayerAbilities = GetComponent<PlayerAbilities>();


        playerAnimator = GetComponent<Animator>();
        health_start = health;
        //set the health slider to the referenced object health amount for on start.
        healthSliderHandle.value = health;
    }



    /// <summary>
    /// Update mothod updates the healthSliderHandle.value to current health and checks if health <= 0.
    /// If health <= 0 then it calls the approporate death related events. 
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //update the health slider value to the referenced object health.
        healthSliderHandle.value = health;

        //checks if health for referenced object is less or equal to 0...
        if(health <= 0)
        {
            //if 0 then...
            //sets the fill area to not active at 0 -> slider value at 0 always has a bit left but we need 0 fill at 0 health.
            healthAtZero.SetActive(false);

            //set animation to dead anim
            playerAnimator.SetBool("dead", true);

            //disable all player actions
            playerController.enabled = false;
            updatePlayerRotation.enabled = false;
            PlayerAbilities.enabled = false;

            //Invoke the PlayerDeathCall() method (what happens upon player death) with 2 sec delay.
            Invoke(nameof(PlayerDeathCall), 2f);
        }
    }


    /// <summary>
    /// OnTriggerEnter checks if collider "other" with a layer of doesDamage hits the player. 
    /// If so it does damage to the player, decrements health (checks if !is_immmune first) and 
    /// then calls the takeDamage sound (iuf health > 0).
    /// </summary>
    /// <param name="other">This refers to the collider that hit the player</param>
    //checks for OnTriggerEnter collision with another object.
    private void OnTriggerEnter(Collider other)
    {
        //So if the other.collider is on layer "doesDamage" and hits the player then...
        if (other.gameObject.layer == 10)
        {
            // ...decrement health only if not is_immune
            if(!is_immune) health -= damageToTake;

            //checks if health is > 0 cause otherwise death sound gets called.
            if(health > 0)
            {
                //player damage sound from the sound manager script
                SoundManager.Instance.PlaySfx(SfxEvent.PlayerDamage);
            }
        }
    }



    /// <summary>
    /// Toggles the value is_immune to either on or off. 
    /// </summary>
    public void Toggle_Is_Immune() {
        if (!is_immune)  StartCoroutine(Immunity_Coroutine());                
    }

    /// <summary>
    /// Keeps the value is_immune to true for 30 seconds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Immunity_Coroutine() { 
        this.is_immune = true;
        yield return new WaitForSeconds(15);
        this.is_immune = false;
    }


    /// <summary>
    /// This handles what happens upon player death: calls the death sound and game over scene
    /// </summary>
    private void PlayerDeathCall()
    {
        //player death sound from the SoundManager script
        //already have the hit noise so don't need this right now
        SoundManager.Instance.PlaySfx(SfxEvent.PlayerDeath);
        //call change of scene to gameover (main menu for now) upon player death.
        SceneManager.LoadScene(SceneName.GameOver.ToString());
    }

    /// <summary>
    /// This method intercepts the addition of health to the player. It ensures that the maximum health remains equal to the start
    /// health.
    /// </summary>
    /// <param name="h">Amount of health to add.</param>
    protected internal void Add_Health(int h) {
        int curr_health = health;
        health = curr_health + h > health_start ? health_start : health + h;       
    }
}

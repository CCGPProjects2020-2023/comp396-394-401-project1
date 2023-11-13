/*
    Author's Name: Alexander  Maynard
    Creation Date: November 11, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: November 11, 2023
    Program Description: This is the simple healthManager script that handles health for various enenmy or playertypes and calls the appropriate death method calls.
    
    Revision History: 
    -November 11, 2023
        -> Added health variables and other object references
        -> Added functionality for the slider to decrease upon the referenced object getting damaged.
        -> added player death and enemy death (empty) methods.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    //health slider object reference
    [Header("Health Slider Reference")]
    public Slider healthSliderHandle;

    //health value for object this scriIt is attached to.
    [Header("Health Attributes of referenced object")]
    public int health = 100;
    public GameObject healthAtZero;

    //set the damage that the referenced object will take
    [Header("How much damage to take")]
    public int damageToTake = 10;

    //set the method call name for invoke dependant on which object reference this script is placed on.
    [Header("Method to invoke on death")]
    public string methodName;




    // Start is called before the first frame update
    void Start()
    {
        //set the health slider to the referenced object health amount for on start.
       healthSliderHandle.value = health;
    }




    // Update is called once per frame
    void Update()
    {
        //update the health slider value to the referenced object health.
        healthSliderHandle.value = health;


        //checks if health for referenced object is 0...
        if(health == 0)
        {
            //if 0 then...
            //sets the fill area to not active at 0 -> slider value at 0 always has a bit left but we need 0 fill at 0 health.
            healthAtZero.SetActive(false);
            //proper method call for referenced object
            Invoke(methodName, 0.5f);
        }
    }


    //checks for OnTriggerEnter collision with another object.
    private void OnTriggerEnter(Collider other)
    {
        //So if layer is doesDamage and hits the referenced object then...
        if (other.gameObject.layer == 10)
        {
            //decrement health
            health -= damageToTake;
        }
    }


    /// <summary>
    /// This handles what happens upon player death.
    /// THis may be called by invoke depending what is set in the editor.
    /// </summary>
    private void PlayerDeathCall()
    {
        //call change of scene to gameover (main menu for now) upon player death.
        SceneManager.LoadScene(SceneName.GameOver.ToString());
    }


    /// <summary>
    /// This handles what happens upon enemy death (if anything at all).
    /// This may be called by invoke depending what is set in the editor.
    /// </summary>
    private void EnemyDeathCall()
    {
        //call enemy death call code
    }
}

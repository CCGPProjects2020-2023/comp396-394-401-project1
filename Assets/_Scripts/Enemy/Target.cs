/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Alexander Maynard
    Last Date Modified:     December 2, 2023
    Program Description:    Sets the type of target to the object.
    Revision History:       November 11, 2023: Initial script and documentation.  
                            December 2, 2023: Changed OnTriggerEnter to a public function to work with raycast.
 */

using UnityEngine;

public class Target : MonoBehaviour
{
    public TargetType type = TargetType.NONE;
    public int multiplier = 1;
    public EnemyController controller;


    /// <summary>
    /// This gets called by a message sent by the PlayerController Shoot() method.
    /// A raycast from playcontroller on the 'Enemy' layer gets sent a then if it hits 
    /// a collider it on that layer it calls this method by the collider.SendMessage() function
    /// This method also looks for the playerController and adjusts the player score, and enemy's health.
    /// </summary>
    public void HitEnemy()
    {
        PlayerController p = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        if (multiplier > 0)
        {
            p.scoreManager.GetComponent<ScoreManager>().Add((int)type * multiplier);
        }
        else p.scoreManager.GetComponent<ScoreManager>().Add((int)type);

        controller.health -= (int)type;
    }
}

/// <summary>
/// Types of target possible.
/// </summary>
public enum TargetType { 
    NONE = 0,
    HEAD = 50,
    BODY = 20,
    ARM = 15,
    LEG = 10
}

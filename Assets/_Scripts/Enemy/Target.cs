/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 11, 2023
    Program Description:    Sets the type of target to the object.
    Revision History:       November 11, 2023: Initial script and documentation.                            
 */

using UnityEngine;

public class Target : MonoBehaviour
{
    public TargetType type = TargetType.NONE;
    public int multiplier = 1;
    private ScoreManager scoreManager;

    /// <summary>
    /// Unity's start function initializing the scoreManager field.
    /// </summary>
    private void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    /// <summary>
    /// Checks whether the other object is a bullet coming from the player.
    /// If it is, it adjusts the player score, and enemy's health.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet")) {
            if(multiplier > 0) scoreManager.Add((int)type * multiplier);
            else  scoreManager.Add((int)type);

            gameObject.transform.root.gameObject.GetComponent<EnemyController>().health -= (int)type;
        }
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

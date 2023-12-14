/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 13, 2023
    Program Description:    Class used to describe a reward
    Revision History:       October 25, 2023: Initial script and documentation.                            
                            December 13, 2023: Added sound when player enters in the reward trigger box.
 */

using UnityEngine;
using System.Collections;

public class Reward : MonoBehaviour
{
    [Header("Reward properties")]
    public int worth;
    public RewardType type;  

    private RewardManager reward_manager;
    private ScoreManager score_manager;
    private HealthManager health_manager;
    private AudioSource audioSource;

    /// <summary>
    /// Start method triggered by unity. This method is used to initialize the managers.
    /// </summary>
    void Start()
    {
        reward_manager = GameObject.Find("RewardManager").GetComponent<RewardManager>();
        score_manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        health_manager = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Method that checks if the other object that collided with this object is the player. It handles the proper 
    /// action based on the reward type.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
            StartCoroutine(WaitForAudio());                               
    }

    IEnumerator WaitForAudio()
    {
        audioSource.Play();
        yield return new WaitForSeconds(1.0f);
        Action();
    }

    /// <summary>
    /// Method that handles the proper action based on the reward type.
    /// </summary>
    private void Action() {
        switch (type)
        {
            case RewardType.HEALTH:
                reward_manager.GetComponent<RewardManager>().health_manager.Add_Health(worth);
                Destroy(gameObject);
                break;

            case RewardType.BONUS:
                score_manager.Add(worth);
                Destroy(gameObject);
                break;

            case RewardType.IMMUNE:                
                Destroy(gameObject);
                health_manager.Toggle_Is_Immune();
                break;

            default:
                break;
        }        
    }
}

/// <summary>
/// Enum that describes the possible types of rewards.
/// </summary>
public enum RewardType { 
    NONE,
    HEALTH,
    IMMUNE,
    BONUS    
}
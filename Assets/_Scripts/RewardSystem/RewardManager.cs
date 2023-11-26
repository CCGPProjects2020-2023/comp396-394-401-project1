/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 25, 2023
    Program Description:    Class used to manage rewards.
    Revision History:       October 25, 2023: Initial script and documentation.                            
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [Header("Reward manager properties")]
    public int numb_rewards_to_spawn;
    public List<Reward> rewards;
    public GameObject points;
    public HealthManager health_manager;

    private readonly Dictionary<int, Reward> reward_dict = new() { };    
    private List<Transform> original_transforms = new();   


    /// <summary>
    /// Start method triggered by unity. This initializes the lists.
    /// </summary>
    /// <exception cref="System.Exception"></exception>
    void Start() {
        if (numb_rewards_to_spawn > points.transform.childCount) 
            throw new System.Exception("Not enough positions to spawn rewards...");        

        Init_lists();
        Init_Spawn();                
    }


    /// <summary>
    /// Initilizes the lists used to manage the rewards.
    /// </summary>
    private void Init_lists() {
        original_transforms = points.transform.GetComponentsInChildren<Transform>().ToList();
        original_transforms.Remove(points.transform);
        for (int i = 0; i < rewards.Count; i++) reward_dict.Add(i, rewards[i]);
    }

    /// <summary>
    /// Instantiates a random reward at a pre-defined location randomly.
    /// </summary>
    private void Init_Spawn() {
        for (int i = 0; i < numb_rewards_to_spawn; i++) {
            System.Random R = new System.Random();
            int rand = R.Next(0, original_transforms.Count - 1);
            Transform t = original_transforms.ElementAt(rand);

            Reward reward = reward_dict.GetValueOrDefault(Random.Range(0, rewards.Count));
            Instantiate(reward, new Vector3(t.position.x, reward.gameObject.GetComponent<Collider>().bounds.max.y + t.position.y, t.position.z), Quaternion.identity);
            original_transforms.Remove(t);
        }
    }
}
/*
    Author's Name: Alexander  Maynard
    Creation Date: November 30, 2023
    Last Modified By: Alexander Maynard
    Last Modified Date: November 30, 2023
    Program Description: This script manages the win condition of the levels. To win there must be 0 enemies in the scene.

    Revision History: 
    -November 30, 2023 
        -> Added first iteration of the WinManager script with basic functionality
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This class tracks how many enemies are in the scene by the ones added to an enemies List that gets enemies added through the editor.
/// </summary>
public class WinManager : MonoBehaviour
{
    //gets the enemies added through the editor.
    [Header("List of All Enemy Objects in the Scene")]
    [SerializeField] private List<GameObject> enemies;

    /// <summary>
    /// OnAwake keeps calling the enemyCheck every 1 second for efficiency sake
    /// </summary>
    public void Awake()
    {
        InvokeRepeating("EnemyCheck", 0.0f, 0.5f * Time.deltaTime);
    }

    /// <summary>
    /// EnemyCheck checks the list of enemies and determiners how many enemies are left. 
    /// The Method then determines if there are 0 enemies left, if this is true then
    /// the player wins, therefore we call the next scene.
    /// </summary>
    private void EnemyCheck()
    {
        //loop through all enemies List items
        foreach(GameObject enemy in enemies)
        {
            // if there is even one enemy in the scene...
            if (enemy != null)
            {
                // ...then just return
                return;
            }
            // if all enemies all destoyed (aka null), the players wins, so call the next scene (level)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

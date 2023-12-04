/*
 * Author's Name:           Alexander  Maynard
 * Creation Date:           November 30, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 3, 2023
 * 
 * Program Description:     This script manages the win condition of the levels. To win there must be 0 enemies in the scene.
 * 
 * Revision History:        November 30, 2023:
 *                              -> Added first iteration of the WinManager script with basic functionality
 *                              
 *                          December 2, 2023:
 *                              -> Fixed bug where Load Scene gets called too early.
 *                              
 *                          December 3, 2023:
 *                              -> Change public variables to private and updated comments/comments headers
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This class tracks how many enemies are in the scene by the ones added to an enemies List (enemies are added through the editor to the List).
/// Then when the List contains 0 enemies, the win condition is acheived and the next scene is called.
/// </summary>
public class WinManager : MonoBehaviour
{

    [Header("List of All Enemy Objects in the Scene")]
    //List of enemies in the scene
    //gets the enemies added through the editor.
    [SerializeField] private List<GameObject> enemies;

    /// <summary>
    /// OnAwake keeps calling (InvokeRepeating) the enemyCheck every 1/2 second for efficiency sake
    /// </summary>
    public void Awake()
    {
        // Repeatedly invokes the EnemyCheck function to check how many enemies are in the List every half second with no delay.
        // OnAwake calls this immediately 
        InvokeRepeating(nameof(EnemyCheck), 0.0f, 0.5f * Time.deltaTime);
    }

    /// <summary>
    /// EnemyCheck checks the list of enemies and determines how many enemies are left. 
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
        }
        // if all enemies all destoyed (aka null), the players wins, so call the next scene (level)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

/*
 * Author's Name:           Alexander  Maynard
 * Creation Date:           November 11, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 3, 2023
 * 
 * Program Description:     This is changes the cursor mode for menus.
 * 
 * Revision History:        November 11, 2023:
 *                              -> Cursor mode setting for menu.
 *                              
 *                          December 3, 2023:
 *                              -> Updated comments/comment headers
 */


using UnityEngine;

/// <summary>
/// This class just sets the cursorLockMode to None so the mouse can freely move across the screen in
/// menus unlike like in game where the mouse in stuck in the center of the screen.
/// </summary>
public class CursorModeMenu : MonoBehaviour
{
    /// <summary>
    /// Start just changes the cursor state.
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        //Unlocks the cursor from the center after the player dies or if entering the game for the first time.
        //This ensures that the plaeyr can freely mode the cursor in menus, unlike in-game.
        Cursor.lockState = CursorLockMode.None;
    }
}

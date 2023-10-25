/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Manages the state of the Main Menu. Will notify
 *                          the presenter when its state has changed.
 *  Revision History:       October 21, 2023: Initial Menu Script.
 *                          October 24, 2023: Added documentation; removed visibility.
 */

using UnityEngine;

/// <summary>
/// The Model for the Main menu.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Quits the game.
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

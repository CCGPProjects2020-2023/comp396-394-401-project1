/*
 * Author's Name:           Alexander  Maynard
 * Creation Date:           December 10, 2023
 * Last Modified By:        Marcus Ngooi
 * Last Modified Date:      December 13, 2023
 * 
 * Program Description:     This script handles the InGamePauseMenu being called and it's functionality like calling main menu and resume.
 * 
 * Revision History:        December 10 2023: 
 *                              -> Added the intial version of this script which include being able to pause/resume with ESC key, resume/quit btn functionality and main menu btn.
 *                              -> Added the proper comments/comment headers.
 *                              -> Added functionality to pause through another btn to pause the menu and changed cursorlock mode depending on if paused or not.
 *                              -> Refactored the pause menu to only pause on tab and resume with the resume button.
 *                              -> Made the isPaused bool public get and private set to restrict setting to only this class but to be able to be read from other classes.
 *                              -> Fixed bug for not moving once you pressing Pause -> enter Main Menu -> hit play. 
 *                          December 13, 2023 (Marcus Ngooi):
 *                              -> Reset score on going back to main menu.
 */

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
/// <summary>
/// This class handles all the InGamePauseMenu functionality, including pausing/resuming through esc or buttons and returning to the main menu.
/// </summary>
public class InGamePauseMenu : MonoBehaviour
{
    //isPaused bool. Private setter for this class and public to be called by other scripts.
    public bool isPaused { get; private set; } 

    //GameObject reference to the ingamePause menu.
    [SerializeField] private GameObject _inGamePauseMenu;

    /// <summary>
    /// set isPaused to false to start off.
    /// </summary>
    void Awake()
    {
        isPaused = false;
    }

    /// <summary>
    /// Update just checks if the ESC key is pressed to appply it's functionality.
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //if we press ESC key then...
        if (Input.GetKey(KeyCode.Tab)) {
            isPaused = true;
            //call PauseGameCheck();
            PauseGameCheck();
        }
    }

    /// <summary>
    /// PauseGameCheck(); reassigns then checks the 
    /// _isPaused bool, to see if it should pause 
    /// the scene and set the pause menu active or not.
    /// </summary>
    private void PauseGameCheck()
    {
        //checks the value of paused
        switch (isPaused)
        {
            //if scene should be paused...
            case true:
                //...pause the scene and set the pause menu as active
                Time.timeScale = 0.0f;
                //unlock the cursor to use the menu
                Cursor.lockState = CursorLockMode.None;
                _inGamePauseMenu.SetActive(true);
                break;
            case false:
                //...resume the scene and set the pause menu as inactive
                Time.timeScale = 1.0f;
                Cursor.lockState = CursorLockMode.Locked;
                //relock the cursor to use the menu
                _inGamePauseMenu.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// If the ResumeGameBtn in the scene is called then this function gets called.
    /// Calls the private PauseGame functionality again to up-pause the scene.
    /// </summary>
    public void ResumeGameBtn()
    {
        //set is paused to false to resume the game.
        isPaused = false;
        //call this again as _isPaused will be inverted anyway to resume the game in PauseGameCheck();
        PauseGameCheck();
    }

    // <summary>
    /// If the Main Menu Btn in the scene is called then this function gets called.
    /// CallMainMenuBtn, just calls the MainMenuBtn function.
    /// </summary>
    public void CallMainMenuBtn()
    {
        //this is needed otherwise if we pause then go to main menu and then back to level the player no longer moves unless you hit pause first.
        Time.timeScale = 1.0f;
        //call the MainMenuBtn functionality
        MainMenuBtn();
    }



    /// <summary>
    /// This is the functionality called by the MainMenuBtn.
    /// it only Loads the Main Menu scene.
    /// </summary>
    private void MainMenuBtn()
    {
        //reset score
        ScoreManager.Score = 0;
        //load the main menu scene
        SceneManager.LoadScene(0);
    }
}

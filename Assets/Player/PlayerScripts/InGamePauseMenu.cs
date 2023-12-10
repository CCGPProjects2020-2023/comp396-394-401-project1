/*
 * Author's Name:           Alexander  Maynard
 * Creation Date:           December 10, 2023
 * Last Modified By:        Alexander Maynard
 * Last Modified Date:      December 10, 2023
 * 
 * Program Description:     This script handles the InGamePauseMenu being called and it's functionality like calling main menu and resume.
 * 
 * Revision History:        December 10 2023: 
 *                              -> Added the intial version of this script which include being able to pause/resume with ESC key, resume/quit btn functionality and main menu btn.
 *                              -> Added the proper comments/comment headers.
 */

using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This class handles all the InGamePauseMenu functionality, including pausing/resuming through esc or buttons and returning to the main menu.
/// </summary>
public class InGamePauseMenu : MonoBehaviour
{
    //isPaused bool
    private bool _isPaused = false;
    //GameObject reference to the ingamePause menu.
    [SerializeField] private GameObject _inGamePauseMenu;

    /// <summary>
    /// Update just checks if the ESC key is pressed to appply it's functionality.
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //if we press ESC key then...
        if (Input.GetKey(KeyCode.Escape)) {
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
        //inverts the value of isPaused as ESC may be called multiple times (to pause/unpause).
        _isPaused = !_isPaused;
        //checks the value of paused
        switch (_isPaused)
        {
            //if scene should be paused...
            case true:
                //...pause the scene and set the pause menu as active
                Time.timeScale = 0.0f;
                _inGamePauseMenu.SetActive(true);
                break;
            case false:
                //...resume the scene and set the pause menu as inactive
                Time.timeScale = 1.0f;
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
        //call this again as _isPaused will be inverted anyway to resume the game in PauseGameCheck();
        PauseGameCheck();
    }




    // <summary>
    /// If the Main Menu Btn in the scene is called then this function gets called.
    /// CallMainMenuBtn, just calls the MainMenuBtn function.
    /// </summary>
    public void CallMainMenuBtn()
    {
        //call the MainMenuBtn functionality
        MainMenuBtn();
    }



    /// <summary>
    /// This is the functionality called by the MainMenuBtn.
    /// it only Loads the Main Menu scene.
    /// </summary>
    private void MainMenuBtn()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("MainMenu").buildIndex);
    }
}

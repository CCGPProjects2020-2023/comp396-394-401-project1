/** Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Receives the user inputs via UI events (e.g., Button click)
 *                          and, in turn manipulates the Main Menu's data (state).
 *  Revision History:       October 21, 2023: Initial MainMenuPresenter script.
 *                          October 24, 2023: Bug fixes and removed visitbility; added Instructions event handler.
 */

using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// The Presenter for the Main menu.
/// </summary>
public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (mainMenu != null)
        {
            // Subscribe to events.
        }
        UpdateView();
    }
    private void OnDestroy()
    {
        if (mainMenu != null)
        {
            // Unsubscribe from events.
        }
    }
    /// <summary>
    /// Event handler for Play button.
    /// </summary>
    public void OnPlayButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.LevelOne.ToString());
    }
    /// <summary>
    /// Event handler for Instructions button.
    /// </summary>
    public void OnInstructionsButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.Instructions.ToString());
    }
    /// <summary>
    /// Event handler for Options button.
    /// </summary>
    public void OnOptionsButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.OptionsMenu.ToString());
    }
    /// <summary>
    /// Event handler for Quit button
    /// </summary>
    public void OnQuitButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        mainMenu.Quit();
    }
    /// <summary>
    /// Updates the view of the Main Menu.
    /// </summary>
    public void UpdateView()
    {
        if (mainMenu == null) return;
        // Update view
    }
}

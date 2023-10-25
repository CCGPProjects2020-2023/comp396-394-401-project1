/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Receives the user inputs via UI events (e.g., Button click)
 *                          and, in turn manipulates the Options Menu's data (state).
 *  Revision History:       October 24, 2023: Initial Options Menu presenter script.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The Presenter for the Options Menu.
/// </summary>
public class OptionsMenuPresenter : MonoBehaviour
{
    [SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private Canvas optionsMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        if(optionsMenu != null)
        {
            // Subscribe to events
        }
        UpdateView();
    }
    private void OnDestroy()
    {
        if (optionsMenu != null)
        {
            // Unsubscribe from events
        }
    }
    /// <summary>
    /// Handles the event: Menu button clicked.
    /// </summary>
    public void OnMenuButtonClicked()
    {
        SceneManager.LoadScene(SceneName.Menu.ToString());
    }
    /// <summary>
    /// Updates the view of the Options Menu.
    /// </summary>
    public void UpdateView()
    {
        if (optionsMenu == null) return;
        // Update view
    }
}

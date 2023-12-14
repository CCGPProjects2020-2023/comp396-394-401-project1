/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Receives the user inputs via UI events (e.g., Button click)
 *                          and, in turn manipulates the Instruction's data (state).
 *  Revision History:       October 24, 2023: Initial InstructionsPresenter script.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The Presenter for the Instructions.
/// </summary>
public class InstructionsPresenter : MonoBehaviour
{
    [SerializeField] private Instructions instructions;

    // Start is called before the first frame update
    void Start()
    {
        if (instructions != null)
        {
            // Subscribe to events.
        }
        UpdateView();
    }
    // OnDestroy is called when the game object is destroyed.
    private void OnDestroy()
    {
        if (instructions != null)
        {
            // Unsubscribe from events.
        }
    }
    /// <summary>
    /// Event handler for Main Menu button.
    /// </summary>
    public void OnMainMenuButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.MainMenu.ToString());
    }
    /// <summary>
    /// Updates the view of the Instructions.
    /// </summary>
    public void UpdateView()
    {
        if (instructions == null) return;
        // Update view
    }
}

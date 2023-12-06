/** Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     November 12, 2023
 *  Program Description:    Receives the user inputs via UI events (e.g., Button click)
 *                          and, in turn communicates with the appropriate managers.
 *  Revision History:       November 12, 2023: Initial GameOverPresenter script.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The Presenter for the Game Over screen.
/// </summary>
public class GameOverPresenter : MonoBehaviour
{
    /// <summary>
    /// Event handler for MainMenu button
    /// </summary>
    public void OnMainMenuButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.MainMenu.ToString());
    }
}

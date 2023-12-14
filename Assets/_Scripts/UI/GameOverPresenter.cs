/** Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     December 13, 2023
 *  Program Description:    Receives the user inputs via UI events (e.g., Button click)
 *                          and, in turn communicates with the appropriate managers.
 *  Revision History:       November 12, 2023 (Marcus Ngooi): Initial GameOverPresenter script.
 *                          December 13, 2023 (Marcus Ngooi): Added Play Again button.
 *                                                            Added score summary.
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// The Presenter for the Game Over screen.
/// </summary>
public class GameOverPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private readonly string scoreString = "Score: ";

    private void Start()
    {
        // Populate score text with player's score.
        scoreText.text = scoreString + ScoreManager.Score.ToString();
    }

    /// <summary>
    /// Event handler for MainMenu button
    /// </summary>
    public void OnMainMenuButtonClicked()
    {
        ScoreManager.Score = 0;
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.MainMenu.ToString());
    }
    /// <summary>
    /// Event handler for Play Again button
    /// </summary>
    public void OnPlayAgainButtonClicked()
    {
        ScoreManager.Score = 0;
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.LevelOneV2.ToString());
    }
}

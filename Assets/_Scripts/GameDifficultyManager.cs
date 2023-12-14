/** Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     December 13, 2023
 *  Program Description:    Changes the stats of the enemies based on the difficulty chosen.
 *  Revision History:       December 13, 2023 (Marcus Ngooi): Initial GameDifficultyManager script.
 */

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the game difficulty.
/// </summary>
public class GameDifficultyManager : Singleton<GameDifficultyManager>
{
    [SerializeField] private float displayTime = 1.5f;

    private GameObject feedbackPanel;
    private TextMeshProUGUI feedbackText;

    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    // Debug. Default game difficulty is EASY.
    [SerializeField] private GameDifficulty currentGameDifficulty = GameDifficulty.EASY;

    public GameDifficulty CurrentGameDifficulty { get { return currentGameDifficulty; } }
    /// <summary>
    /// Awake method called by Unity. It adds an event listener on scene loaded.
    /// </summary>
    private void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Triggers logic when a scene loads. This function was made specifically to handle the feedback panel.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneName.OptionsMenu.ToString())
        {
            if (feedbackPanel == null)
            {
                feedbackPanel = GameObject.Find("FeedbackPanel");
                feedbackText = GameObject.Find("FeedbackText").GetComponent<TextMeshProUGUI>();
                feedbackPanel.SetActive(false);
            }

            if (easyButton == null)
            {
                easyButton = GameObject.Find("EasyDifficultyButton").GetComponent<Button>();
                easyButton.onClick.AddListener(() => OnEasyButtonClicked());
            }
            if (mediumButton == null)
            {
                mediumButton = GameObject.Find("MediumDifficultyButton").GetComponent<Button>();
                mediumButton.onClick.AddListener(() => OnMediumButtonClicked());
            }
            if (hardButton == null)
            {
                hardButton = GameObject.Find("HardDifficultyButton").GetComponent<Button>();
                hardButton.onClick.AddListener(() => OnHardButtonClicked());
            }
        }
    }
    /// <summary>
    /// Handles logic when Easy button clicked.
    /// </summary>
    public void OnEasyButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        currentGameDifficulty = GameDifficulty.EASY;
        PresentPlayerFeedback();
    }
    /// <summary>
    /// Handles logic when Medium button clicked.
    /// </summary>
    public void OnMediumButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        currentGameDifficulty = GameDifficulty.MEDIUM;
        PresentPlayerFeedback();
    }
    /// <summary>
    /// Handles logic when Hard button clicked.
    /// </summary>
    public void OnHardButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        currentGameDifficulty = GameDifficulty.HARD;
        PresentPlayerFeedback();
    }
    /// <summary>
    /// Provides feedback to player.
    /// </summary>
    private void PresentPlayerFeedback()
    {
        feedbackText.text = $"{currentGameDifficulty} difficulty selected!";
        StartCoroutine(ShowAndHide(displayTime));
    }
    /// <summary>
    /// Shows the feedback window for a set amount of time.
    /// </summary>
    /// <param name="displayTime"></param>
    /// <returns></returns>
    IEnumerator ShowAndHide(float displayTime)
    {
        feedbackPanel.SetActive(true);
        feedbackPanel.GetComponent<CanvasGroup>().alpha = 1.0f;
        yield return new WaitForSeconds(displayTime);
        feedbackPanel.GetComponent<CanvasGroup>().alpha = 0f;
        feedbackPanel.SetActive(false);
    }
}

/// <summary>
/// Available difficulties.
/// </summary>
public enum GameDifficulty
{
    EASY,
    MEDIUM,
    HARD
}

/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Receives the user inputs via UI events (e.g., Button click)
 *                          and, in turn manipulates the Options Menu's data (state).
 *  Revision History:       October 24, 2023: Initial Options Menu presenter script.
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The Presenter for the Options Menu.
/// </summary>
public class OptionsMenuPresenter : MonoBehaviour
{
    [SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private Canvas optionsMenuCanvas;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if(optionsMenu != null)
        {
            // Subscribe to events
        }

        // Initialize sliders
        musicVolumeSlider.value = SoundManager.Instance.MusicVolume;
        sfxVolumeSlider.value = SoundManager.Instance.SfxVolume;

        // Add listeners for sliders
        musicVolumeSlider.onValueChanged.AddListener(optionsMenu.ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(optionsMenu.ChangeSfxVolume);

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
    public void OnMainMenuButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.MainMenu.ToString());
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

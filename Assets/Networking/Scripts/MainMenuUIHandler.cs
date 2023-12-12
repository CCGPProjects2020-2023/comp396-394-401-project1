/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles MainMenu UI
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{

    public GameObject playerDetailsPanel;
    public GameObject sessionDetailsPanel;
    public GameObject createSessionDetailsPanel;
    public GameObject statusPanel;

    public TMP_InputField playerNameInputField;
    public TMP_InputField sessionNameInputField;

    /// <summary>
    /// Start method called by unity - Sets the input field text to player prefs' nickname
    /// </summary>
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerNickname"))
        {
            playerNameInputField.text = PlayerPrefs.GetString("PlayerNickname");
        }
    }

    /// <summary>
    /// Hides all panels
    /// </summary>
    void HideAllPanels() { 
        playerDetailsPanel.SetActive(false);
        sessionDetailsPanel.SetActive(false);
        createSessionDetailsPanel.SetActive(false);
        statusPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles the action of the Find Game button
    /// </summary>
    public void OnFindGameClicked() {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        PlayerPrefs.SetString("PlayerNickname", playerNameInputField.text);
        PlayerPrefs.Save();

        GameManager.instance.playerNickName = playerNameInputField.text;

        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        
        networkRunnerHandler.OnJoinLobby();

        HideAllPanels();
        
        sessionDetailsPanel.SetActive(true);

        FindObjectOfType<SessionListUIHandler>(true).OnLookingForGameSession();
    }

    /// <summary>
    /// Handles the action of the back button
    /// </summary>
    public void OnBackButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.ModeMenu.ToString());
    }

    /// <summary>
    /// Handles the actions of the create new game button
    /// </summary>
    public void OnCreateNewGameClicked() {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        HideAllPanels();
        createSessionDetailsPanel.SetActive(true);
    }

    /// <summary>
    /// Handles the actions of the start new session button
    /// </summary>
    public void OnStartNewSessionClicked() {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        networkRunnerHandler.CreateGame(sessionNameInputField.text, "Multiplayer");

        HideAllPanels();

        statusPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles the actions of the Join button
    /// </summary>
    public void OnJoiningServer() {
        HideAllPanels();

        statusPanel.gameObject.SetActive(true);
    }
}
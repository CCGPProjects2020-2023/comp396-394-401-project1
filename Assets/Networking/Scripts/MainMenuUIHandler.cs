using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerNickname"))
        {
            playerNameInputField.text = PlayerPrefs.GetString("PlayerNickname");
        }
    }

    void HideAllPanels() { 
        playerDetailsPanel.SetActive(false);
        sessionDetailsPanel.SetActive(false);
        createSessionDetailsPanel.SetActive(false);
        statusPanel.gameObject.SetActive(false);
    }

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

    public void OnBackButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        SceneManager.LoadScene(SceneName.ModeMenu.ToString());
    }

    public void OnCreateNewGameClicked() {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        HideAllPanels();
        createSessionDetailsPanel.SetActive(true);
    }

    public void OnStartNewSessionClicked() {
        SoundManager.Instance.PlaySfx(SfxEvent.ButtonClick);
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        networkRunnerHandler.CreateGame(sessionNameInputField.text, "Multiplayer");

        HideAllPanels();

        statusPanel.gameObject.SetActive(true);
    }

    public void OnJoiningServer() {
        HideAllPanels();

        statusPanel.gameObject.SetActive(true);
    }
}

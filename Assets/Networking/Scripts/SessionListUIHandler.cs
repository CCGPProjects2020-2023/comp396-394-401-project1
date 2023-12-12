/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles the UI to update the session list items
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube

using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionListUIHandler : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public GameObject sessionItemListPrefab;
    public VerticalLayoutGroup verticalLayoutGroup;

    /// <summary>
    /// Awake method called by unity - Clears the session list
    /// </summary>
    private void Awake()
    {
        ClearList();
    }

    /// <summary>
    /// Clears the session list
    /// </summary>
    public void ClearList() { 
        foreach(Transform child in verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        statusText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Adds an item to the session list
    /// </summary>
    /// <param name="session"></param>
    public void AddToList(SessionInfo session) { 
        SessionInfoListUIItem item = Instantiate(sessionItemListPrefab, verticalLayoutGroup.transform).GetComponent<SessionInfoListUIItem>();
        item.SetInformation(session);

        item.OnJoinSession += AddedSessionInfoListUIItem_OnJoinSession;
    }

    /// <summary>
    /// Added a session to the list of current session available
    /// </summary>
    /// <param name="session"></param>
    private void AddedSessionInfoListUIItem_OnJoinSession(SessionInfo session)
    {
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        networkRunnerHandler.JoinGame(session);
        MainMenuUIHandler mainMenu = FindObjectOfType<MainMenuUIHandler>();
        mainMenu.OnJoiningServer();
    }

    /// <summary>
    /// Handles actions when no session is found
    /// </summary>
    public void OnNoSessionsFound() {

        ClearList();
        statusText.text = "No game session found";
        statusText.gameObject.SetActive(true);
    }

    public void OnLookingForGameSession()
    {
        ClearList();
        statusText.text = "Looking for a game session";
        statusText.gameObject.SetActive(true);
    }
}
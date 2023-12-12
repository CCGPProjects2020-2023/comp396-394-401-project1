/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Handles the action of a session list item in the UI
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System;

public class SessionInfoListUIItem : MonoBehaviour
{
    public TextMeshProUGUI sessionNametext;
    public TextMeshProUGUI playerCountText;
    public Button joinButton;

    SessionInfo sessionInfo;

    public event Action<SessionInfo> OnJoinSession;

    /// <summary>
    /// Sets the information of the session on the UI
    /// </summary>
    /// <param name="session"></param>
    public void SetInformation(SessionInfo session)
    { 
        this.sessionInfo= session;
        sessionNametext.text = session.Name;
        playerCountText.text = $"{session.PlayerCount.ToString()}/{session.MaxPlayers.ToString()}";

        bool isJoinButtonActive = true;

        if(session.PlayerCount >= session.MaxPlayers)
            isJoinButtonActive = false;

        joinButton.gameObject.SetActive(isJoinButtonActive);
    }

    /// <summary>
    /// Handles the click button to join a lession
    /// </summary>
    public void OnClick() { 
        OnJoinSession?.Invoke(sessionInfo);
    }
}
using System.Collections;
using System.Collections.Generic;
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

    public void OnClick() { 
        OnJoinSession?.Invoke(sessionInfo);
    }
}

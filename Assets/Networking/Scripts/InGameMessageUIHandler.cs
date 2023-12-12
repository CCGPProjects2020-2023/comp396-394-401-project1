/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    In game UI Handler
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube

using System.Collections;
using TMPro;
using UnityEngine;

public class InGameMessageUIHandler : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshProUGUIs;
    Queue messageQueue = new Queue();   
    
    /// <summary>
    /// Handles messages in game
    /// </summary>
    /// <param name="message"></param>
    public void OnGameMessageReceived(string message) {
        Debug.Log($"InGameMessagesUIHandler {message}");

        messageQueue.Enqueue( message );

        if(messageQueue.Count > 3) 
            messageQueue.Dequeue();

        int queueIndex = 0;
        foreach(string messageInQueue in messageQueue)
        {
            textMeshProUGUIs[queueIndex].text = messageInQueue;
            queueIndex++;
        }
    }
}
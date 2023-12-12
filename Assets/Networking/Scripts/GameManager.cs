/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    GameManager - takes care of the connection token.
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    byte[] connectionToken;

    public Vector2 cameraViewRotation = Vector2.zero;
    public string playerNickName = "";

    /// <summary>
    /// Awake method called by unity - Checks that only one instance of this exists.
    /// </summary>
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }    

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Start method called by unity - Sets the connection token
    /// </summary>
    void Start()
    {
        if (connectionToken == null)
        {
            connectionToken = ConnectionTokenUtils.NewToken();
            Debug.Log($"Player connection token {ConnectionTokenUtils.HashToken(connectionToken)}");
        }
    }
    
    /// <summary>
    /// Sets the connection token to the parameter
    /// </summary>
    /// <param name="connectionToken"></param>
    public void SetConnectionToken(byte[] connectionToken)
    {
        this.connectionToken = connectionToken;
    }

    /// <summary>
    /// Returns the connection token
    /// </summary>
    /// <returns></returns>
    public byte[] GetConnectionToken() {
        return this.connectionToken;
    }
}
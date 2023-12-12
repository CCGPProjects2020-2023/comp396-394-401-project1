using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    byte[] connectionToken;

    public Vector2 cameraViewRotation = Vector2.zero;
    public string playerNickName = "";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }    

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (connectionToken == null)
        {
            connectionToken = ConnectionTokenUtils.NewToken();
            Debug.Log($"Player connection token {ConnectionTokenUtils.HashToken(connectionToken)}");
        }
    }
    
    public void SetConnectionToken(byte[] connectionToken)
    {
        this.connectionToken = connectionToken;
    }

    public byte[] GetConnectionToken() {
        return this.connectionToken;
    }
}

/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 11, 2023
    Program Description:    Manages the score for the player.
    Revision History:       November 11, 2023: Initial script and documentation.                            
 */
using TMPro;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{
    private readonly string s_score = "Score: ";
    private static int _score = 0;
    public TextMeshProUGUI score_text;

    /// <summary>
    /// Getter and Setter for the property Score
    /// It checks if the score we are trying to set is abvove 0.
    /// </summary>
    public static int Score { set {
            if (value > 0) _score = value;
            else throw new System.Exception("Trying to set a score to something that is less than or equal to 0...");
    } get { return _score; } }

    /// <summary>
    /// Unity's start function to initialize the score text.
    /// </summary>
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        score_text.text = s_score + Score.ToString();
    }

    /// <summary>
    /// Increases the Score property by the score parameter.
    /// </summary>
    /// <param name="score"></param>
    protected internal void Add(int score) {
        int temp = _score;
        temp += score;
        Score = temp;
        score_text.text = s_score + Score.ToString();
    }

    /// <summary>
    /// Decreses the Score property by the score parameter.
    /// </summary>
    /// <param name="score"></param>
    protected internal void Remove(int score) {
        int temp = _score;
        temp -= score;
        Score = temp;        
        score_text.text = s_score + Score.ToString();
    }
}
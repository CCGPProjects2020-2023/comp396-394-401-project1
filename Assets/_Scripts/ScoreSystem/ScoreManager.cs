/**
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Marcus Ngooi
    Last Date Modified:     December 13, 2023
    Program Description:    Manages the score for the player.
    Revision History:       November 11, 2023 (Audrey Bernier Larose): Initial script and documentation. 
                            December 13, 2023 (Marcus Ngooi): Added a score reset function and allowed for setting a score back to 0 on play again.
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
    /// It checks if the score we are trying to set is 0 or above 0.
    /// </summary>
    public static int Score { set {
            if (value >= 0) _score = value;
            else throw new System.Exception("Trying to set a score to something that is less than 0...");
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
    /// <summary>
    /// Resets the Score property to 0.
    /// </summary>
    /// <param name="score"></param>
    protected internal void Reset()
    {
        Score = 0;
        score_text.text = s_score + Score.ToString();
    }
}
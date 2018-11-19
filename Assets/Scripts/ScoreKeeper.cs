using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Counts and displays score
public class ScoreKeeper : MonoBehaviour {

    public static int kills {get; private set;}

    GameUI gameUI;

    // Use this for initialization
    void Start () {

        //PlayerPrefs.DeleteAll();

        gameUI = GameObject.Find("Canvas").GetComponent<GameUI>();
        if (gameUI == null) {
            Debug.LogError("ScoreKeeper: GameUI missing");
        }
    }

    public void IncreaseScore() { 
        kills++;
    }

    public void IncreaseScore(int amount) {
        kills += amount;
    }

    public void RestartScore() {
        kills = 0;
    }

    // ---------- Highscores -------------

    public void SubmitNewHighscore(int score, string gun) {

        if (PlayerPrefs.HasKey(gun + "HighScore")) {
            if (score > PlayerPrefs.GetInt(gun + "HighScore")) {
                PlayerPrefs.SetInt(gun + "HighScore", score);

                gameUI.NewHighScore(true); 
            }
            else {
                gameUI.NewHighScore(false);
            }
        }
        else {
            PlayerPrefs.SetInt(gun + "HighScore", score);
            gameUI.NewHighScore(true);
        }

        if (score > PlayerPrefs.GetInt("TotalHighScore")) {
            PlayerPrefs.SetInt("TotalHighScore", score);
        }
    }

    public static int GetTotalHightScore() {
        return PlayerPrefs.GetInt("TotalHighScore");
    }

    public static int GetHighscore(string gun) {
        return PlayerPrefs.GetInt(gun + "HighScore");
    }
}

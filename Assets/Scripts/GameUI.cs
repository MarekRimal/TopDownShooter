using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {


    public Text gameScore;
    public Text hpBanner;
    public Text firstSpellCD;
    public Text secondSpellCD;

    public Text totalHighScore;
    public Text gunScores;
    public Text highscoreText;
    public Image sceneBlur;

    public GameObject gameOverStuff;
    public Text gameOverScore;

    Player player;
    ICharacter character;

    public float blurTime;
    public float maxBlur;

    public event System.Action OnPlayAgainButtonPressed;

    private void Start() {

        player = FindObjectOfType<Player>();
        character = player.gameObject.GetComponent<ICharacter>();

        gameObject.transform.SetParent(null);

        sceneBlur.CrossFadeAlpha(0, blurTime, false);
        gameOverStuff.SetActive(false);
    }

    void Update () {

        string firstSpellName = character.GetFisrtSpellName();
        string secondSpellName = character.GetSecondSpellName();

        int firstCD = (int) character.GetFisrtSpellCD();
        int secondCD = (int) character.GetSecondSpellCD();

        gameScore.text = "KILLS: " + ScoreKeeper.kills.ToString();
        firstSpellCD.text = firstSpellName +  ": " + firstCD.ToString();
        secondSpellCD.text = secondSpellName + ": " + secondCD.ToString();

        if (player != null) {
            hpBanner.text = "HP: " + player.getCurrHealth().ToString();
        }
    }

    // Will show new highScore banner
    public void NewHighScore(bool highScore) {

        if (highScore) {
            highscoreText.text = "!!NEW HIGHSCORE!!";
        }
        else {
            highscoreText.text = "HIGHSCORE: " + ScoreKeeper.GetTotalHightScore();
        }
    }

    // When player dies
    public void GameOver() {

        gameScore.CrossFadeAlpha(0, blurTime, false);
        hpBanner.CrossFadeAlpha(0, blurTime, false);
        sceneBlur.CrossFadeAlpha(maxBlur, blurTime, false);

        Invoke("EnableGameOverStuff", blurTime);
        //StartCoroutine(EnableGameOverStuff());
    }

    // Enable stuff to display after players death
    void EnableGameOverStuff() {

        gameOverStuff.SetActive(true);
        gameOverScore.text = "KILLS: " + ScoreKeeper.kills.ToString();
        gunScores.text =
            "Pistol: " + ScoreKeeper.GetHighscore("Pistol") +
            "\nSMG: " + ScoreKeeper.GetHighscore("SMG") +
            "\nSniper: " + ScoreKeeper.GetHighscore("Sniper") +
            "\nCannon: " + ScoreKeeper.GetHighscore("Cannon") +
            "\nMines: " + ScoreKeeper.GetHighscore("Mines") +
            "\nShotgun: " + ScoreKeeper.GetHighscore("Shotgun");
        totalHighScore.text = "Total HighScore: \n" + ScoreKeeper.GetTotalHightScore();

    }

    public void PlayAgainButtonPressed() {
        Application.LoadLevel(Application.loadedLevel);
        //OnPlayAgainButtonPressed();
    }
}
   

    //IEnumerator EnableGameOverStuff() {

    //    yield return new WaitForSeconds(blurTime);
    //    gameOverStuff.gameObject.SetActive(true);
    //    gameOverStuff.SetActive(true);
    //    print("GameOverStuff active = " + gameOverStuff.activeSelf);


    //    gameOverScore.text = gameScore.text;
    //}

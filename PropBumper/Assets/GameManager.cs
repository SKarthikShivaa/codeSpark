using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject startPanel, inGamePanel,endGamePanel;
    public AudioListener camerAudioListener;
    public  Text inGameScore, endGameScore, endGameHighscore, audioButton;
   int audioOn;
    public int score, highscore;

    public enum GameState
        {
        GameStart,
        GameRunning,
        GameEnd
        };

    public GameState game;

    public void Awake ()
        {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        }

	// Use this for initialization
	void Start () {

        inGamePanel.SetActive(false);
        endGamePanel.SetActive(false);
        score = 0;

        highscore = PlayerPrefs.GetInt("Highscore");
        audioOn = PlayerPrefs.GetInt("AudioOn");
        AudioOnOff();

        }

    public void GameStart ()
        {
        game = GameState.GameRunning;
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
        PlatformScript.instance.StartPlatforms();   //Platforms moves up and randomDrops
        EnemyDropper.instance.StartEnemySpawns();   //Drops enemies - Object pooled
        StartCoroutine(GameLoop());
        }


    IEnumerator GameLoop ()
        {
        yield return StartCoroutine(GamePlaying());
        yield return StartCoroutine(GameEnding());
        }

    IEnumerator GamePlaying ()
        {
        while (game == GameState.GameRunning)
            {
            score++;
            inGameScore.text = "Score: " + score.ToString();
            yield return new WaitForSeconds(1f);
            }
        }

    IEnumerator GameEnding ()
        {
        if (score >= highscore)
            {
            highscore = score;
            PlayerPrefs.SetInt("Highscore",score);
            }
        endGameScore.text = "Score: " + score.ToString();
        endGameHighscore.text = "Highscore: " + highscore.ToString();

        endGamePanel.SetActive(true);
        yield return new WaitForSeconds(5f);
        GameRestart();
        }

	// Update is called once per frame
	void Update () {
	
	}

    public void GameEnd ()
        {
        game = GameState.GameEnd;
       
        }

    public void GameRestart ()
        {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    

    public void AudioOnOff ()
        {
        PlayerPrefs.SetInt("AudioOn",audioOn);
        if (audioOn == 1)
            {
            camerAudioListener.enabled = false;
            audioButton.text = "Sound on";
            audioOn = 0;
            }
        else
            {
            camerAudioListener.enabled = true;
            audioButton.text = "Sound off";
            audioOn = 1;
            }
     

        }
}

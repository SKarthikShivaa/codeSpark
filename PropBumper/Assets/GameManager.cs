using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject startPanel, inGamePanel,endGamePanel;

    public  Text inGameScore, endGameScore, endGameHighscore;

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

        }

    public void GameStart ()
        {
        game = GameState.GameRunning;
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
        PlatformScript.instance.StartPlatforms();   //Platforms moves up and randomDrops
        EnemyDropper.instance.StartEnemySpawns();   //Drops enemies - Object pooled
        StartCoroutine(ScoreKeeper());
        }

	// Update is called once per frame
	void Update () {
	
	}

    public void GameEnd ()
        {
        game = GameState.GameEnd;
        if (score >= highscore)
            {
            highscore = score;
            PlayerPrefs.SetInt("Highscore",score);
            }
        endGameScore.text = "Score: " + score.ToString();
        endGameHighscore.text = "Highscore: " + highscore.ToString();

        endGamePanel.SetActive(true);
        }

    public void GameRestart ()
        {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    public IEnumerator ScoreKeeper ()
        {
        while (game == GameState.GameRunning)
            {
            score++;
            inGameScore.text = "Score: " + score.ToString();
            yield return new WaitForSeconds(1f);
            }
        }
}

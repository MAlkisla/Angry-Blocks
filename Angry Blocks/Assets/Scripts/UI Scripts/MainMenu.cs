using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public Text scoreText;
    public Text bestScoreText;

    private GameController gameController;
    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    void Update()
    {
        scoreText.text = gameController.score.ToString();

        if (gameController.score > PlayerPrefs.GetInt("BestScore",0))
        {
            PlayerPrefs.SetInt("BestScore", gameController.score);
        }
        bestScoreText.text = "Best  " + PlayerPrefs.GetInt("BestScore");
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //or
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
    }
}

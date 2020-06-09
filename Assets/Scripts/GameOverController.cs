using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverUI;

    [Header("SFX")]
    public string ThemeMusic;

    void Start()
    {
        FindObjectOfType<PlayerController>().OnDeath += OnGameOver;
    }

    void OnGameOver()
    {
        Cursor.visible = true;
        // Time.timeScale = 0;
        FindObjectOfType<AudioManager>().Play("ScreenDeath");
        gameOverUI.SetActive(true);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FindObjectOfType<AudioManager>().Play(ThemeMusic);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverUI;

    [Header("SFX")]
    public AudioClip ScreenMusic;

    void Start()
    {
        FindObjectOfType<PlayerController>().OnDeath += OnGameOver;
    }

    void OnGameOver()
    {
        // AudioController.instance.Pause();
        // AudioController.instance.PlayOneShot(ScreenMusic);
        // FindObjectOfType<AudioManager>().
        Cursor.visible = true;
        Time.timeScale = 0;
        FindObjectOfType<AudioManager>().Play("ScreenDeath");
        // AudioController.instance.priority = 0;
        gameOverUI.SetActive(true);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FindObjectOfType<AudioManager>().Play("BackgroundTheme");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

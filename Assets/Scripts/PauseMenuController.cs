using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private static bool ConfigurationPaused = false;

    public GameObject PauseMenuUI;
    public GameObject ConfigurationMenuUI;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && ConfigurationPaused == false)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        ConfigurationMenuUI.SetActive(true);
        ConfigurationPaused = true;
    }

    public void ExitConfiguration()
    {
        ConfigurationMenuUI.SetActive(false);
        ConfigurationPaused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}

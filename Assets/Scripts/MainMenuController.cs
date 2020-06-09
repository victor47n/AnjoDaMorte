using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("MenuMusic");
    }

    public void PlayGame()
    {
        LoadingController.CallLoading(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

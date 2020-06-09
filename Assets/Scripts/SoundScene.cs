using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScene : MonoBehaviour
{

    [Header("SFX")]
    public string ThemeMusic;

    void Awake()
    {
        FindObjectOfType<AudioManager>().Play(ThemeMusic);
    }

}

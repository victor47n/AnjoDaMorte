using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource _audioSource;
    public static AudioSource instance;
    
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        instance = _audioSource;
    }

}

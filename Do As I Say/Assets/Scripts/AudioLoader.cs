using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    public Dictionary<string, List<AudioClip>> clip;

    [SerializeField] private List<AudioClip> background;
    [SerializeField] private List<AudioClip> soundEffect;

    private void Awake()
    {
        LoadAudio();
    }

    private void LoadAudio()
    {
        clip = new Dictionary<string, List<AudioClip>>();

        LoadBackgroundMusic();

        LoadSounds();
    }

    private void LoadBackgroundMusic()
    {
        clip.Add("BackgroundMusic", background);
    }


    private void LoadSounds()
    {
        clip.Add("SoundEffects", soundEffect);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicCon : MonoBehaviour
{
    AudioSource music;

    [SerializeField] private AudioLoader audioLoader;

    private List<AudioClip> tracks;

    float currentSongLength;
    int totalTracks;

    private void Start()
    {
        totalTracks = audioLoader.clip["BackgroundMusic"].Count;
        music = GetComponent<AudioSource>();

        tracks = audioLoader.clip["BackgroundMusic"];
        music.clip = tracks[0];
    }

    public void Update()
    {
        if (music.clip != null && music.time >= currentSongLength)
        {
            AudioClip clip = tracks[Random.Range(0, totalTracks)];
            currentSongLength = clip.length;
            music.clip = clip;
            music.Play();
        }
    }

    public void UpdateTrack(AudioClip track)
    {
        music.clip = track;
    }
}

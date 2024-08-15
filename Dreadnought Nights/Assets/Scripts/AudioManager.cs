using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> clips;
    public List<AudioSource> audioSources;

    private AudioSource tempAudioSource;

    public void PlayClip(int clipIndex, float maxPitchOffset)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying) continue;

            source.clip = clips[clipIndex];
            source.pitch = 1 + Random.Range(-maxPitchOffset, maxPitchOffset);
            source.Play();
            break;
        }
    }

    public void LoopClip(int clipIndex)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying) continue;

            source.clip = clips[clipIndex];
            source.loop = true;
            source.Play();

            tempAudioSource = source;

            break;
        }
    }

    public void UnLoopSource()
    {
        if (tempAudioSource == null) return;

        tempAudioSource.loop = false;
        tempAudioSource.Stop();
    }

    internal void PlayClip(int clipIndex, float maxPitchOffset, int specificIndex)
    {
        AudioSource source = audioSources[specificIndex];

        source.clip = clips[clipIndex];
        source.pitch = 1 + Random.Range(-maxPitchOffset, maxPitchOffset);
        source.Play();
    }

    public void LoopClip(int clipIndex, int index)
    {
        AudioSource source = audioSources[index];

        source.clip = clips[clipIndex];
        source.loop = true;
        source.Play();

        tempAudioSource = source;
    }
}

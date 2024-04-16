using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource footstepsSource;
    public AudioClipsSO AudioClips;

    public float BGMVolme { get { return bgmSource.volume; } set { bgmSource.volume = value; } }
    public float SFXVolme { get { return sfxSource.volume; } set { sfxSource.volume = value; } }
    public float FootstepsVolme { get { return footstepsSource.volume; } set { footstepsSource.volume = value; } }

    public void PlayFootsteps(AudioClip clip)
    {
        if(footstepsSource.clip == clip)
            return;

        footstepsSource.clip = clip;
        footstepsSource.Play();
    }

    public void StopFootsteps()
    {
        if (footstepsSource.isPlaying == false)
            return;

        footstepsSource.clip = null;
        footstepsSource.Stop();
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource.isPlaying == false)
            return;

        bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void StopSFX()
    {
        if (sfxSource.isPlaying == false)
            return;

        sfxSource.Stop();
    }
}

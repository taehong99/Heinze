using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioClips", fileName = "AudioClips")]
public class AudioClipsSO : ScriptableObject
{
    [Header("BGMs")]
    public AudioClip lobbyBGM;
    public AudioClip floor1BGM;
    public AudioClip floor2BGM;
    public AudioClip floor3BGM;
    public AudioClip midbossBGM;
    public AudioClip bossBGM;

    [Header("Player")]
    public AudioClip dashSFX;

    [Header("Misc")]
    public AudioClip clickSFX;
    public AudioClip potionDrinkSFX;
}

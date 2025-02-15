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
    public AudioClip attack1SFX;
    public AudioClip attack2SFX;
    public AudioClip attack3SFX;
    public AudioClip walkingSFX;
    public AudioClip runningSFX;
    public AudioClip skill1SFX;
    public AudioClip skill2SFX;
    public AudioClip skill3SFX;
    public AudioClip skill4SFX;
    public AudioClip skill5SFX;
    public AudioClip skill6SFX;
    
    [Header("Monster")]
    public AudioClip explosionSFX;
    public AudioClip monsterHitSFX;

    [Header("Misc")]
    public AudioClip clickSFX;
    public AudioClip potionDrinkSFX;
    public AudioClip openDoorSFX;
    public AudioClip gameOverSFX;
    public AudioClip chestOpenSFX;
    public AudioClip cardSelectSFX;
    public AudioClip cardFlipSFX;
    public AudioClip tabOpenSFX;
    public AudioClip tabCloseSFX;
}

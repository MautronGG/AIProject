using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource m_musicSource;
    [SerializeField] AudioSource m_sfxSource;

    //AudioClip m_musicClip;
    //AudioClip m_sfxClip;

    [Header("Audio Clips")]

    [Header("Music")]
    public AudioClip m_music_Menu;
    public AudioClip m_music_Gameplay;

    [Header("SFX")]

    [Header("Player")]
    public AudioClip m_sfx_DieFall;
    public AudioClip m_sfx_DieEnemy;
    public AudioClip m_sfx_ReachedGoal;

    [Header("Objetcts")]
    public AudioClip m_sfx_Spring;
    public AudioClip m_sfx_Portal;
    public AudioClip m_sfx_PickUp;
    public AudioClip m_sfx_OpenDoor;
    public AudioClip m_sfx_LaserShoot;

    [Header("Misc")]
    public AudioClip m_sfx_ButtonClick;
    public AudioClip m_sfx_PlaceBridge;
    public AudioClip m_sfx_RemoveBridge;
    public AudioClip m_sfx_Spray;
    public AudioClip m_sfx_CantSpray;
    public AudioClip m_sfx_GoalOpen;
    public AudioClip m_sfx_GoalClose;
    public AudioClip m_sfx_Win;
    public AudioClip m_sfx_GameOver;



    public void PlayMusic(AudioClip audioClip)
    {
        if (audioClip == null || m_musicSource == null)
        { 
            return;
        }
        m_musicSource.clip = audioClip;
        m_musicSource.Play();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        if (audioClip == null || m_sfxSource == null)
        {
            return;
        }
        m_sfxSource.clip = audioClip;
        m_sfxSource.Play();
    }
}

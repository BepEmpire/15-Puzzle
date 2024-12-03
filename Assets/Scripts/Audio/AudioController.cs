using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource soundEffectsSource;
    [SerializeField] private AudioSource musicSource;
    
    [Header("Sound and Music Clips")]
    [SerializeField] private AudioClip[] sounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        LoadAudioSettings();
    }

    public void SetSound(bool isOn)
    {
        soundEffectsSource.mute = !isOn;
        PlayerPrefs.SetInt("SoundOn", isOn ? 1 : 0);
    }

    public void SetMusic(bool isOn)
    {
        musicSource.mute = !isOn;
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = FindAudioClipByName(soundName);
        
        if (clip != null && !soundEffectsSource.mute)
        {
            soundEffectsSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"AudioClip {soundName} not found or AudioSource is mute");
        }
    }

    private AudioClip FindAudioClipByName(string soundName)
    {
        foreach (AudioClip audioClip in sounds)
        {
            if (audioClip.name == soundName)
            {
                return audioClip;
            }
        }
        
        return null;
    }

    private void LoadAudioSettings()
    {
        bool soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        bool musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        
        SetSound(soundOn);
        SetMusic(musicOn);
    }
}

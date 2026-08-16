using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private string musicVolumeParameter = "MusicVolume";
    [SerializeField] private string sfxVolumeParameter = "SFXVolume";

    [Header("UI")]
    [SerializeField] private AudioButtonsUI audioButtonsUI;

    private bool musicMuted;
    private bool sfxMuted;

    public bool MusicMuted => musicMuted;
    public bool SFXMuted => sfxMuted;

    public event Action<bool> OnMusicMuteChanged;
    public event Action<bool> OnSFXMuteChanged;

    private void Start()
    {
        // Встановлюємо початкові іконки
        if (audioButtonsUI != null)
        {
            audioButtonsUI.SetMusicState(!musicMuted);
            audioButtonsUI.SetSoundState(!sfxMuted);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            ToggleMusic();

        if (Input.GetKeyDown(KeyCode.X))
            ToggleSFX();
    }

    public void ToggleMusic()
    {
        musicMuted = !musicMuted;

        audioMixer.SetFloat(
            musicVolumeParameter,
            musicMuted ? -80f : 0f
        );

        // Змінюємо іконку музики
        if (audioButtonsUI != null)
            audioButtonsUI.SetMusicState(!musicMuted);

        OnMusicMuteChanged?.Invoke(musicMuted);
    }

    public void ToggleSFX()
    {
        sfxMuted = !sfxMuted;

        audioMixer.SetFloat(
            sfxVolumeParameter,
            sfxMuted ? -80f : 0f
        );

        // Змінюємо іконку звуків
        if (audioButtonsUI != null)
            audioButtonsUI.SetSoundState(!sfxMuted);

        OnSFXMuteChanged?.Invoke(sfxMuted);
    }
}
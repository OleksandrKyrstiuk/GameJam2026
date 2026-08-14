using UnityEngine;
using UnityEngine.Audio;

public class UIAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioMixerGroup outputGroup;

    public static UIAudioPlayer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        if (source == null)
        {
            source = GetComponent<AudioSource>();

            if (source == null)
                source = gameObject.AddComponent<AudioSource>();
        }

        source.playOnAwake = false;
        source.outputAudioMixerGroup = outputGroup;
    }

    public static void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
            return;

        if (Instance == null)
        {
            Debug.LogWarning("UIAudioPlayer: no UIAudio object in scene! Create one on the Canvas with the UIAudioPlayer script.");
            return;
        }

        Debug.Log($"UIAudioPlayer: playing '{clip.name}', volume: {volume}, mute: {Instance.source.mute}, source volume: {Instance.source.volume}, mixer group: {(Instance.source.outputAudioMixerGroup != null ? Instance.source.outputAudioMixerGroup.name : "none")}");

        Instance.source.PlayOneShot(clip, volume);
    }
}

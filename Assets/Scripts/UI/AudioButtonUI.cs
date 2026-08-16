using UnityEngine;
using UnityEngine.UI;

public class AudioButtonsUI : MonoBehaviour
{
    [Header("Music Button")]
    [SerializeField] private Image musicButtonImage;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    [Header("Sound Button")]
    [SerializeField] private Image soundButtonImage;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    public void SetMusicState(bool isOn)
    {
        musicButtonImage.sprite = isOn
            ? musicOnSprite
            : musicOffSprite;
    }

    public void SetSoundState(bool isOn)
    {
        soundButtonImage.sprite = isOn
            ? soundOnSprite
            : soundOffSprite;
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;

    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverClip != null)
            UIAudioPlayer.Play(hoverClip, volume * 0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"UIButtonSound: click on '{gameObject.name}', clip: {(clickClip != null ? clickClip.name : "NULL")}");

        if (clickClip != null)
            UIAudioPlayer.Play(clickClip, volume);
    }
}

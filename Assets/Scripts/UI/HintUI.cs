using System.Collections;
using TMPro;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip voiceClip;

    [Header("Timing")]
    [SerializeField] private float fadeInTime = 0.4f;
    [SerializeField] private float displayTime = 2.5f;
    [SerializeField] private float fadeOutTime = 0.4f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        hintPanel.SetActive(false);
    }

    public void ShowHint(string message)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowHintRoutine(message));
    }

    private IEnumerator ShowHintRoutine(string message)
    {
        hintText.text = message;

        hintPanel.SetActive(true);

        // Програємо голос один раз
        if (audioSource != null && voiceClip != null)
        {
            audioSource.PlayOneShot(voiceClip);
        }

        // Плавна поява
        yield return StartCoroutine(
            Fade(0f, 1f, fadeInTime)
        );

        // Показуємо текст
        yield return new WaitForSeconds(displayTime);

        // Плавне зникнення
        yield return StartCoroutine(
            Fade(1f, 0f, fadeOutTime)
        );

        hintPanel.SetActive(false);

        currentCoroutine = null;
    }

    private IEnumerator Fade(float start, float target, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);

            canvasGroup.alpha = Mathf.Lerp(start, target, t);

            yield return null;
        }

        canvasGroup.alpha = target;
    }
}
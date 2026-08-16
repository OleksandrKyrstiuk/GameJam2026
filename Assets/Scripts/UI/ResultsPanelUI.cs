using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsPanelUI : MonoBehaviour
{
    [Header("Results Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text mistakesText;
    [SerializeField] private TMP_Text tasksText;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text commentText;

    [SerializeField] private string mainMenuSceneName;

    [Header("Ending")]
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private CanvasGroup endingCanvasGroup;
    [SerializeField] private AudioSource endingAudio;

    [SerializeField] private float delayBeforeEnding = 2f;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float delayBeforeAudio = 3f;

    [Header("Penalty")]
    [SerializeField] private float penaltyPerMistake = 1f;

    [Header("Rank thresholds (seconds)")]
    [SerializeField] private float rankS = 90f;
    [SerializeField] private float rankA = 120f;
    [SerializeField] private float rankB = 150f;
    [SerializeField] private float rankC = 180f;
    [SerializeField] private float rankD = 240f;
    [SerializeField] private float rankE = 300f;

    private void Awake()
    {

        if (panel != null)
            panel.SetActive(false);

        if (endingCanvasGroup != null)
            endingCanvasGroup.alpha = 0f;
    }

    public void Show(float elapsedSeconds, int mistakes, int totalTasks)
    {
        float penalty = mistakes * penaltyPerMistake;
        float totalTime = elapsedSeconds + penalty;

        string rank = GetRank(totalTime);

        int minutes = (int)(totalTime / 60f);
        float seconds = totalTime - minutes * 60f;

        if (timeText != null)
        {
            timeText.text =
                $"Час: {minutes:00}:{seconds:00.00}" +
                (penalty > 0f ? $" (+{penalty:0} с штрафу)" : "");
        }

        if (mistakesText != null)
            mistakesText.text = $"Помилки: {mistakes} (-{penalty:0} с)";

        if (tasksText != null)
            tasksText.text = $"Завдання: {totalTasks}/{totalTasks}";

        if (rankText != null)
            rankText.text = rank;

        if (commentText != null)
            commentText.text = GetComment(rank);

        StartCoroutine(ShowEndingSequence());
    }

    private IEnumerator ShowEndingSequence()
    {
        // 1. Чекаємо 2 секунди після виконання всіх завдань
        yield return new WaitForSeconds(2f);

        // 2. Плавно показуємо чорну панель
        if (endingCanvasGroup != null)
        {
            yield return StartCoroutine(
                FadeCanvasGroup(endingCanvasGroup, 0f, 1f, fadeInTime)
            );
        }

        // 3. Чекаємо ще 3 секунди
        yield return new WaitForSeconds(delayBeforeAudio);

        // 4. Програємо фінальний звук
        if (endingAudio != null)
            endingAudio.Play();

        // 5. Чекаємо 4 секунди
        yield return new WaitForSeconds(4f);

        // 6. Показуємо результати
        if (panel != null)
            panel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator FadeCanvasGroup(
        CanvasGroup canvasGroup,
        float start,
        float target,
        float duration)
    {
        float time = 0f;

        canvasGroup.alpha = start;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);

            canvasGroup.alpha = Mathf.Lerp(start, target, t);

            yield return null;
        }

        canvasGroup.alpha = target;
    }

    private string GetRank(float seconds)
    {
        if (seconds <= rankS) return "S";
        if (seconds <= rankA) return "A";
        if (seconds <= rankB) return "B";
        if (seconds <= rankC) return "C";
        if (seconds <= rankD) return "D";
        if (seconds <= rankE) return "E";

        return "F";
    }

    private string GetComment(string rank)
    {
        switch (rank)
        {
            case "S":
                return "Ідеально. Навіть підозріло.";

            case "A":
                return "Майже ідеально. Паніка пройшла за планом.";

            case "B":
                return "Непогано. Можна вважати ранок врятованим.";

            case "C":
                return "Не поїсти, не помитися, зате виглядати пристойно. Пріоритети зрозумілі.";

            case "D":
                return "Головне, що з квартири таки вдалося вийти.";

            case "E":
                return "Ранок пережито. Уже непогано.";

            case "F":
                return "Ранок пережито. Уже непогано.";

            default:
                return "Помянемо...";
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text mistakesText;
    [SerializeField] private TMP_Text tasksText;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text commentText;

    [SerializeField] private string mainMenuSceneName;

    [Header("Penalty")]
    [SerializeField] private float penaltyPerMistake = 1f;

    [Header("Rank thresholds (seconds)")]
    [SerializeField] private float rankS = 90f;
    [SerializeField] private float rankA = 120f;
    [SerializeField] private float rankB = 150f;
    [SerializeField] private float rankC = 180f;
    [SerializeField] private float rankD = 240f;
    [SerializeField] private float rankE = 300f;

    public void Show(float elapsedSeconds, int mistakes, int totalTasks)
    {
        float penalty = mistakes * penaltyPerMistake;
        float totalTime = elapsedSeconds + penalty;

        string rank = GetRank(totalTime);

        int minutes = (int)(totalTime / 60f);
        float seconds = totalTime - minutes * 60f;

        if (timeText != null)
            timeText.text =
                $"Час: {minutes:00}:{seconds:00.00}" +
                (penalty > 0f ? $" (+{penalty:0} с штрафу)" : "");

        if (mistakesText != null)
            mistakesText.text =
                $"Помилки: {mistakes} (-{penalty:0} с)";

        if (tasksText != null)
            tasksText.text =
                $"Завдання: {totalTasks}/{totalTasks}";

        if (rankText != null)
            rankText.text = rank;

        if (commentText != null)
            commentText.text = GetComment(rank);

        panel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private string GetRank(float seconds)
    {
        if (seconds <= rankS) return "S";
        if (seconds <= rankA) return "A";
        if (seconds <= rankB) return "B";
        if (seconds <= rankC) return "C";

        return "D";
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

            default:
                return "Ранок пережито. Уже непогано.";
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
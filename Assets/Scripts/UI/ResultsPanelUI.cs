using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text mistakesText;
    [SerializeField] private TMP_Text tasksText;
    [SerializeField] private string mainMenuSceneName;

    public void Show(float elapsedSeconds, int mistakes, int totalTasks)
    {
        int minutes = (int)(elapsedSeconds / 60f);
        float seconds = elapsedSeconds - minutes * 60f;

        if (timeText != null)
            timeText.text = $"Час: {minutes:00}:{seconds:00.00}";

        if (mistakesText != null)
            mistakesText.text = $"Помилки: {mistakes}";

        if (tasksText != null)
            tasksText.text = $"Завдання: {totalTasks}/{totalTasks}";

        panel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}

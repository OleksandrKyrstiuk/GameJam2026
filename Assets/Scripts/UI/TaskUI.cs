using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] taskTexts;

    [Header("Current Task")]
    [SerializeField] private Color currentTaskColor = new Color(1f, 0.9f, 0.7f);
    [SerializeField] private float currentTaskFontSizeBonus = 2f;

    private float defaultFontSize;
    private Color defaultColor;

    private void Awake()
    {
        if (taskTexts.Length == 0)
            return;

        defaultFontSize = taskTexts[0].fontSize;
        defaultColor = taskTexts[0].color;
    }

    public void UpdateTasks(Task[] tasks, int currentTaskIndex)
    {
        for (int i = 0; i < taskTexts.Length; i++)
        {
            // Якщо для цього поля немає завдання
            if (i >= tasks.Length)
            {
                taskTexts[i].text = "";
                taskTexts[i].fontSize = defaultFontSize;
                taskTexts[i].color = defaultColor;
                taskTexts[i].fontStyle = FontStyles.Normal;

                continue;
            }

            taskTexts[i].text = tasks[i].taskDescription;

            // Виконане завдання
            if (i < currentTaskIndex)
            {
                taskTexts[i].fontSize = defaultFontSize;
                taskTexts[i].color = defaultColor;
                taskTexts[i].fontStyle = FontStyles.Strikethrough;
            }
            // Поточне завдання
            else if (i == currentTaskIndex)
            {
                taskTexts[i].fontSize =
                    defaultFontSize + currentTaskFontSizeBonus;

                taskTexts[i].color = currentTaskColor;
                taskTexts[i].fontStyle = FontStyles.Normal;
            }
            // Майбутнє завдання
            else
            {
                taskTexts[i].fontSize = defaultFontSize;
                taskTexts[i].color = defaultColor;
                taskTexts[i].fontStyle = FontStyles.Normal;
            }
        }
    }
}
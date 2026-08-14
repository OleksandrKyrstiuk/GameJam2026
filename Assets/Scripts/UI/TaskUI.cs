using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] taskTexts;

    public void UpdateTasks(Task[] tasks, int currentTaskIndex)
    {
        for (int i = 0; i < taskTexts.Length; i++)
        {
            if (i >= tasks.Length)
            {
                taskTexts[i].text = "";
                taskTexts[i].color = Color.white;
                taskTexts[i].fontStyle = FontStyles.Normal;

                continue;
            }

            taskTexts[i].text = tasks[i].taskDescription;

            if (i < currentTaskIndex)
            {
                // Виконане завдання
                taskTexts[i].fontStyle = FontStyles.Strikethrough;
                taskTexts[i].color = Color.white;
            }
            else if (i == currentTaskIndex)
            {
                // Поточне завдання
                taskTexts[i].fontStyle = FontStyles.Normal;
                taskTexts[i].color = Color.green;
            }
            else
            {
                // Майбутнє завдання
                taskTexts[i].fontStyle = FontStyles.Normal;
                taskTexts[i].color = Color.white;
            }
        }
    }
}
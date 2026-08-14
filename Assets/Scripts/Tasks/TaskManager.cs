using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskData taskData;
    [SerializeField] private TaskUI taskUI;

    private int currentTaskIndex;

    private void Start()
    {
        currentTaskIndex = 0;

        UpdateTaskUI();
    }

    public void CheckObject(string objectID)
    {
        if (currentTaskIndex >= taskData.tasks.Count)
            return;

        Task currentTask = taskData.tasks[currentTaskIndex];

        // TODO:
        // Тут пізніше можна зробити додаткову перевірку:
        // чи правильний об'єкт, чи правильний етап,
        // чи виконані необхідні умови тощо.

        if (objectID == currentTask.requiredObjectID)
        {
            CompleteCurrentTask();
        }
        else
        {
            Debug.Log($"Wrong object! Required: {currentTask.requiredObjectID}");
        }
    }

    private void CompleteCurrentTask()
    {
        Debug.Log($"Task completed: {taskData.tasks[currentTaskIndex].taskDescription}");

        currentTaskIndex++;

        UpdateTaskUI();

        if (currentTaskIndex >= taskData.tasks.Count)
        {
            CompleteAllTasks();
        }
    }

    private void UpdateTaskUI()
    {
        List<Task> currentStageTasks = new List<Task>();

        if (currentTaskIndex < taskData.tasks.Count)
        {
            TaskStage currentStage = taskData.tasks[currentTaskIndex].stage;

            for (int i = 0; i < taskData.tasks.Count; i++)
            {
                if (taskData.tasks[i].stage == currentStage)
                {
                    currentStageTasks.Add(taskData.tasks[i]);
                }
            }

            int stageTaskIndex = 0;

            for (int i = 0; i < currentTaskIndex; i++)
            {
                if (taskData.tasks[i].stage == currentStage)
                {
                    stageTaskIndex++;
                }
            }

            taskUI.UpdateTasks(
                currentStageTasks.ToArray(),
                stageTaskIndex
            );
        }
    }

    private void CompleteAllTasks()
    {
        Debug.Log("ALL TASKS COMPLETED!");

        // TODO:
        // Тут можна завершити гру або перейти
        // до наступного етапу.
    }

    // TEMPORARY UI TEST
    public void TestObject(string objectID)
    {
        CheckObject(objectID);
    }
}
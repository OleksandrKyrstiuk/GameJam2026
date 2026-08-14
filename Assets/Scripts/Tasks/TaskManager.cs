using System.Collections.Generic;
using UnityEngine;

public  class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskData taskData;
    [SerializeField] private TaskUI taskUI;
    [SerializeField] private StopwatchTimer stopwatch;
    [SerializeField] private ResultsPanelUI resultsPanel;

    public static TaskManager Instance { get; private set; }

    private int currentTaskIndex;
    private int mistakesCount;

    public int MistakesCount => mistakesCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentTaskIndex = 0;

        UpdateTaskUI();
    }

    // DEBUG: press F10 in Play mode to instantly finish all tasks
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10) && currentTaskIndex < taskData.tasks.Count)
        {
            currentTaskIndex = taskData.tasks.Count - 1;

            CompleteCurrentTask();
        }
    }

    public void CheckObject(string objectID)
    {
        if (currentTaskIndex >= taskData.tasks.Count)
            return;

        Task currentTask = taskData.tasks[currentTaskIndex];

        // TODO:
        // ��� ������ ����� ������� ��������� ��������:
        // �� ���������� ��'���, �� ���������� ����,


        if (objectID == currentTask.requiredObjectID)
        {
            CompleteCurrentTask();
        }
        else
        {
            mistakesCount++;

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

        float finalTime = 0f;

        if (stopwatch != null)
        {
            stopwatch.StopTimer();
            finalTime = stopwatch.Elapsed;
        }

        if (resultsPanel != null)
            resultsPanel.Show(finalTime, mistakesCount, taskData.tasks.Count);
        else
            Debug.LogWarning("Results Panel is not assigned in TaskManager!");
    }

    // TEMPORARY UI TEST
    public void TestObject(string objectID)
    {
        CheckObject(objectID);
    }
}
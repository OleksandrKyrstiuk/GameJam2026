using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private TaskData taskData;
    [SerializeField] private TaskUI taskUI;
    [SerializeField] private StopwatchTimer stopwatch;
    [SerializeField] private ResultsPanelUI resultsPanel;
    [SerializeField] private HintUI hintUI;
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

    public bool CheckObject(string objectID)
    {
        if (currentTaskIndex >= taskData.tasks.Count)
            return false;

        Task currentTask = taskData.tasks[currentTaskIndex];

        if (objectID == currentTask.requiredObjectID)
        {
            CompleteCurrentTask();

            return true;
        }
        else
        {
            mistakesCount++;

            Debug.Log($"Wrong object! Required: {currentTask.requiredObjectID}");

            return false;
        }
    }

    private void CompleteCurrentTask()
    {
        Debug.Log($"Task completed: {taskData.tasks[currentTaskIndex].taskDescription}");

        TaskStage completedStage = taskData.tasks[currentTaskIndex].stage;

        currentTaskIndex++;

        // Перевіряємо, чи почався новий етап
        if (currentTaskIndex < taskData.tasks.Count)
        {
            TaskStage nextStage = taskData.tasks[currentTaskIndex].stage;

            if (completedStage != nextStage)
            {
                ShowStageHint(nextStage);
            }
        }

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

    private void ShowStageHint(TaskStage nextStage)
    {
        switch (nextStage)
        {
            case TaskStage.Bathroom:
                hintUI.ShowHint("Зуби самі себе не почистять.");
                break;

            case TaskStage.Kitchen:
                hintUI.ShowHint("Кава сама себе не зробить.");
                break;

            case TaskStage.LivingRoom:
                hintUI.ShowHint("Здається одяг залишився у вітальні");
                break;

            case TaskStage.Hallway:
                hintUI.ShowHint("Схоже, час вже зибратись і на вихід.");
                break;
        }
    }
}

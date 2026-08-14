using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "TaskData",
    menuName = "Game/Task Data"
)]
public class TaskData : ScriptableObject
{
    public List<Task> tasks = new List<Task>();
}

[Serializable]

public class Task
{
    public string taskDescription;
    public TaskStage stage;
    public string requiredObjectID;
    public bool isCompleted;
}

public enum TaskStage
{
    Bedroom,
    Bathroom,
    Kitchen,
    LivingRoom,
    Hallway
}
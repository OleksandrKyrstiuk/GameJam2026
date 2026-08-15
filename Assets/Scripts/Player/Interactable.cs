using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string objectID;
    [SerializeField] private string promptText;

    public string PromptText => string.IsNullOrEmpty(promptText) ? objectID : promptText;

    public virtual void Interact()
    {
        if (TaskManager.Instance != null)
            TaskManager.Instance.CheckObject(objectID);
        else
            Debug.LogWarning("TaskManager not found in scene!");
    }
}

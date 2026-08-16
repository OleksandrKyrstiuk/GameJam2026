using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string objectID;
    [SerializeField] private string promptText;

    public string PromptText => string.IsNullOrEmpty(promptText) ? objectID : promptText;

    public virtual bool Interact()
    {
        if (TaskManager.Instance != null)
            return TaskManager.Instance.CheckObject(objectID);

        Debug.LogWarning("TaskManager not found in scene!");
        return false;
    }
}

using UnityEngine;

public class RadioInteractable : Interactable
{
    [Header("Radio")]
    [SerializeField] private AudioSource radioSource;
    [SerializeField] private AudioClip melodyClip;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    private bool isOn;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        ToggleRadio();

        return taskDone;
    }

    private void ToggleRadio()
    {
        if (radioSource == null)
        {
            Debug.LogWarning($"RadioInteractable on '{gameObject.name}': Radio Source is not assigned!", this);
            return;
        }

        isOn = !isOn;

        if (isOn)
        {
            if (melodyClip != null && radioSource.clip == null)
                radioSource.clip = melodyClip;

            radioSource.Play();
        }
        else
        {
            radioSource.Stop();
        }
    }
}

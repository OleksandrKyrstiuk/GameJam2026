using UnityEngine;

public class ZipperInteractable : Interactable
{
    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip zipperClip;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    [SerializeField] private bool playOnce = true;

    private bool alreadyPlayed;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        PlayZipper();

        return taskDone;
    }

    private void PlayZipper()
    {
        if (playOnce && alreadyPlayed)
            return;

        if (audioSource == null || zipperClip == null)
        {
            Debug.LogWarning($"ZipperInteractable on '{gameObject.name}': Audio Source or Zipper Clip is not assigned!", this);
            return;
        }

        alreadyPlayed = true;

        audioSource.PlayOneShot(zipperClip);
    }
}

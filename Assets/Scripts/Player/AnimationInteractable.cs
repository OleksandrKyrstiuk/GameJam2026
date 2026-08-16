using System.Collections;
using UnityEngine;

public class AnimationInteractable : Interactable
{
    [Header("Animation Target")]
    [SerializeField] private Animation targetAnimation;
    [SerializeField] private AnimationClip openClip;
    [SerializeField] private AnimationClip closeClip;
    [SerializeField] private bool playOnce = true;

    [Header("Object Toggle Target (light etc.)")]
    [SerializeField] private GameObject toggleObject;
    [SerializeField] private bool startToggledOn = true;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    [Header("Revert on wrong timing")]
    [SerializeField] private bool revertOnWrongTask = true;
    [SerializeField] private float autoRevertDelay = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioToStop;
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private AudioSource pressAudioSource;
    [SerializeField] private bool audioOnce = true;

    private bool isOpen;
    private bool alreadyUsed;
    private bool audioStopped;
    private Coroutine revertRoutine;

    private void Start()
    {
        if (toggleObject != null && startToggledOn)
            isOpen = true;
    }

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask && !(playOnce && alreadyUsed))
            taskDone = base.Interact();

        bool animPlayed = PlayAnimation();
        HandleAudio();

        if (animPlayed && revertOnWrongTask && countAsTask && !taskDone)
        {
            if (revertRoutine != null)
                StopCoroutine(revertRoutine);

            revertRoutine = StartCoroutine(RevertAfterDelay());
        }
        else if (animPlayed && revertRoutine != null)
        {
            StopCoroutine(revertRoutine);
            revertRoutine = null;
        }

        return taskDone;
    }

    private IEnumerator RevertAfterDelay()
    {
        AnimationClip playedClip = isOpen ? closeClip : openClip;

        float wait = autoRevertDelay;

        if (playedClip != null)
            wait += playedClip.length;

        yield return new WaitForSeconds(wait);

        PlayAnimation(ignoreOnceLock: true);
        alreadyUsed = false;

        revertRoutine = null;
    }

    private void HandleAudio()
    {
        if (audioToStop != null && (!audioOnce || !audioStopped))
        {
            audioToStop.Stop();
            audioStopped = true;
        }

        if (pressClip != null && pressAudioSource != null)
            pressAudioSource.PlayOneShot(pressClip);
    }

    private bool PlayAnimation(bool ignoreOnceLock = false)
    {
        if (!ignoreOnceLock && playOnce && alreadyUsed)
            return false;

        bool hasAnimation = targetAnimation != null && (isOpen ? closeClip != null : openClip != null);
        bool hasToggle = toggleObject != null;

        if (!hasAnimation && !hasToggle)
        {
            Debug.LogWarning($"AnimationInteractable on '{gameObject.name}': nothing to do! Assign Target Animation or Toggle Object.", this);
            return false;
        }

        if (hasAnimation)
        {
            AnimationClip clip = isOpen ? closeClip : openClip;

            targetAnimation.AddClip(clip, clip.name);
            targetAnimation.Play(clip.name);
        }

        isOpen = !isOpen;
        alreadyUsed = true;

        if (hasToggle)
            toggleObject.SetActive(isOpen);

        return true;
    }
}

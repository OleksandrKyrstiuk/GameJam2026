using UnityEngine;

public class AnimationInteractable : Interactable
{
    [Header("Animation Target")]
    [SerializeField] private Animation targetAnimation;
    [SerializeField] private AnimationClip openClip;
    [SerializeField] private AnimationClip closeClip;
    [SerializeField] private bool playOnce = true;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    private bool isOpen;
    private bool alreadyUsed;

    public override void Interact()
    {
        if (countAsTask && !(playOnce && alreadyUsed))
            base.Interact();

        PlayAnimation();
    }

    private void PlayAnimation()
    {
        if (targetAnimation == null)
        {
            Debug.LogWarning($"AnimationInteractable on '{gameObject.name}': Target Animation is not assigned!", this);
            return;
        }

        if (playOnce && alreadyUsed)
            return;

        AnimationClip clip = isOpen ? closeClip : openClip;

        if (clip == null)
        {
            Debug.LogWarning($"AnimationInteractable on '{gameObject.name}': {(isOpen ? "Close" : "Open")} Clip is not assigned!", this);
            return;
        }

        targetAnimation.AddClip(clip, clip.name);
        targetAnimation.Play(clip.name);

        isOpen = !isOpen;
        alreadyUsed = true;
    }
}

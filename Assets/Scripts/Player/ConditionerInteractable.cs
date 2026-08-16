using UnityEngine;

public class ConditionerInteractable : Interactable
{
    [Header("AC Sound")]
    [SerializeField] private AudioSource acSource;
    [SerializeField] private AudioClip humClip;

    [Header("AC Effect (child, starts hidden)")]
    [SerializeField] private ParticleSystem acParticles;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    private bool isOn;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        ToggleConditioner();

        return taskDone;
    }

    private void ToggleConditioner()
    {
        isOn = !isOn;

        if (acSource != null)
        {
            if (isOn)
            {
                if (humClip != null && acSource.clip == null)
                    acSource.clip = humClip;

                acSource.loop = true;
                acSource.Play();
            }
            else
            {
                acSource.Stop();
            }
        }
        else
        {
            Debug.LogWarning($"ConditionerInteractable on '{gameObject.name}': AC Source is not assigned!", this);
        }

        if (acParticles != null)
        {
            if (isOn)
            {
                if (!acParticles.gameObject.activeSelf)
                    acParticles.gameObject.SetActive(true);

                acParticles.Play();
            }
            else
            {
                acParticles.Stop();

                if (acParticles.gameObject.activeSelf)
                    acParticles.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning($"ConditionerInteractable on '{gameObject.name}': AC Particles is not assigned!", this);
        }
    }
}

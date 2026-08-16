using System.Collections;
using UnityEngine;

public class MicrowaveInteractable : Interactable
{
    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip coffeeClip;

    [Header("Steam (starts hidden)")]
    [SerializeField] private ParticleSystem steamParticles;

    [Header("Timing")]
    [SerializeField] private float duration = 5f;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    private bool isWorking;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        if (!isWorking && (!countAsTask || taskDone))
            StartCoroutine(WorkSequence());

        return taskDone;
    }

    private IEnumerator WorkSequence()
    {
        isWorking = true;

        float wait = duration;

        if (audioSource != null && coffeeClip != null)
        {
            audioSource.PlayOneShot(coffeeClip);

            if (coffeeClip.length > wait)
                wait = coffeeClip.length;
        }

        if (steamParticles != null)
        {
            if (!steamParticles.gameObject.activeSelf)
                steamParticles.gameObject.SetActive(true);

            steamParticles.Play();
        }

        yield return new WaitForSeconds(wait);

        if (steamParticles != null)
        {
            steamParticles.Stop();

            if (steamParticles.gameObject.activeSelf)
                steamParticles.gameObject.SetActive(false);
        }

        isWorking = false;
    }
}

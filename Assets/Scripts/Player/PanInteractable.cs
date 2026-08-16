using System.Collections;
using UnityEngine;

public class PanInteractable : Interactable
{
    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sizzleClip;

    [Header("Pancake (starts hidden)")]
    [SerializeField] private GameObject pancakeObject;

    [Header("Sparkles (starts hidden)")]
    [SerializeField] private ParticleSystem sparklesParticles;
    [SerializeField] private float sparklesDuration = 3f;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    private bool isWorking;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        if (!isWorking && (!countAsTask || taskDone))
            StartCoroutine(CookSequence());

        return taskDone;
    }

    private IEnumerator CookSequence()
    {
        isWorking = true;

        float wait = sparklesDuration;

        if (audioSource != null && sizzleClip != null)
        {
            audioSource.PlayOneShot(sizzleClip);

            if (sizzleClip.length > wait)
                wait = sizzleClip.length;
        }

        if (pancakeObject != null && !pancakeObject.activeSelf)
            pancakeObject.SetActive(true);

        if (sparklesParticles != null)
        {
            if (!sparklesParticles.gameObject.activeSelf)
                sparklesParticles.gameObject.SetActive(true);

            sparklesParticles.Play();
        }

        yield return new WaitForSeconds(wait);

        if (sparklesParticles != null)
        {
            sparklesParticles.Stop();

            if (sparklesParticles.gameObject.activeSelf)
                sparklesParticles.gameObject.SetActive(false);
        }

        isWorking = false;
    }
}

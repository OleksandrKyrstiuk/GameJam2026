using UnityEngine;

public class CoolerInteractable : Interactable
{
    [Header("Dishes to clean (disappear)")]
    [SerializeField] private GameObject[] dishes;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip waterClip;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    [SerializeField] private bool cleanOnce = true;

    private bool alreadyCleaned;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        CleanDishes();

        return taskDone;
    }

    private void CleanDishes()
    {
        if (cleanOnce && alreadyCleaned)
            return;

        if (dishes == null || dishes.Length == 0)
        {
            Debug.LogWarning($"CoolerInteractable on '{gameObject.name}': Dishes list is empty!", this);
            return;
        }

        alreadyCleaned = true;

        foreach (GameObject dish in dishes)
        {
            if (dish != null && dish.activeSelf)
                dish.SetActive(false);
        }

        if (audioSource != null && waterClip != null)
            audioSource.PlayOneShot(waterClip);
        else
            Debug.LogWarning($"CoolerInteractable on '{gameObject.name}': Audio Source or Water Clip is not assigned!", this);
    }
}

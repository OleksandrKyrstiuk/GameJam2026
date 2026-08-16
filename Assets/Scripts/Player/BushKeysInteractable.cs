using System.Collections;
using UnityEngine;

public class BushKeysInteractable : Interactable
{
    [Header("Keys Image (UI, starts hidden)")]
    [SerializeField] private GameObject keysImage;

    [Header("Timing")]
    [SerializeField] private float showDuration = 2.5f;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    [SerializeField] private bool showOnce = true;

    private bool alreadyShown;
    private Coroutine showRoutine;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        ShowKeys();

        return taskDone;
    }

    private void ShowKeys()
    {
        if (showOnce && alreadyShown)
            return;

        if (keysImage == null)
        {
            Debug.LogWarning($"BushKeysInteractable on '{gameObject.name}': Keys Image is not assigned!", this);
            return;
        }

        alreadyShown = true;

        if (showRoutine != null)
            StopCoroutine(showRoutine);

        showRoutine = StartCoroutine(ShowAndHide());
    }

    private IEnumerator ShowAndHide()
    {
        keysImage.SetActive(true);

        yield return new WaitForSeconds(showDuration);

        keysImage.SetActive(false);

        showRoutine = null;
    }
}

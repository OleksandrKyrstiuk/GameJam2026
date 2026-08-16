using System.Collections;
using UnityEngine;

public class BreakfastInteractable : Interactable
{
    [Header("Camera Focus")]
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private float cameraMoveTime = 0.7f;

    [Header("Items that MOVE to the table")]
    [SerializeField] private GameObject coffeeCup;
    [SerializeField] private Transform coffeeCupPoint;
    [SerializeField] private GameObject plate;
    [SerializeField] private Transform platePoint;

    [Header("Pancake (child of plate, eaten in the end)")]
    [SerializeField] private GameObject pancake;

    [Header("Steam (child of cup)")]
    [SerializeField] private ParticleSystem coffeeSteam;

    [Header("Sound")]
    [SerializeField] private AudioSource chewSource;
    [SerializeField] private AudioClip chewClip;

    [Header("Timing")]
    [SerializeField] private float eatDuration = 6f;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    private bool isEating;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        if (!isEating && (!countAsTask || taskDone))
            StartCoroutine(BreakfastSequence());

        return taskDone;
    }

    private IEnumerator BreakfastSequence()
    {
        isEating = true;

        Camera cam = Camera.main;
        Controlerr player = null;
        Vector3 camPos = Vector3.zero;
        Quaternion camRot = Quaternion.identity;

        if (cam != null && cameraPoint != null)
        {
            camPos = cam.transform.localPosition;
            camRot = cam.transform.localRotation;

            player = cam.GetComponentInParent<Controlerr>();

            if (player != null)
                player.enabled = false;

            Transform parent = cam.transform.parent;

            Vector3 targetPos = parent.InverseTransformPoint(cameraPoint.position);
            Quaternion targetRot = Quaternion.Inverse(parent.rotation) * cameraPoint.rotation;

            yield return MoveCamera(cam.transform, targetPos, targetRot, cameraMoveTime);
        }

        if (coffeeCup != null && coffeeCupPoint != null)
        {
            coffeeCup.transform.SetPositionAndRotation(coffeeCupPoint.position, coffeeCupPoint.rotation);
        }

        if (plate != null && platePoint != null)
        {
            plate.transform.SetPositionAndRotation(platePoint.position, platePoint.rotation);
        }

        if (pancake != null && !pancake.activeSelf)
            pancake.SetActive(true);

        if (coffeeSteam != null)
        {
            if (!coffeeSteam.gameObject.activeSelf)
                coffeeSteam.gameObject.SetActive(true);

            coffeeSteam.Play();
        }

        if (chewSource != null && chewClip != null)
        {
            chewSource.loop = true;
            chewSource.clip = chewClip;
            chewSource.Play();
        }

        yield return new WaitForSeconds(eatDuration);

        if (chewSource != null)
            chewSource.Stop();

        if (pancake != null)
            pancake.SetActive(false);

        if (coffeeSteam != null)
        {
            coffeeSteam.Stop();

            if (coffeeSteam.gameObject.activeSelf)
                coffeeSteam.gameObject.SetActive(false);
        }

        if (cam != null && cameraPoint != null)
        {
            yield return MoveCamera(cam.transform, camPos, camRot, cameraMoveTime);

            if (player != null)
                player.enabled = true;
        }

        isEating = false;
    }

    private IEnumerator MoveCamera(Transform t, Vector3 targetPos, Quaternion targetRot, float duration)
    {
        Vector3 startPos = t.localPosition;
        Quaternion startRot = t.localRotation;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float k = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            t.localPosition = Vector3.Lerp(startPos, targetPos, k);
            t.localRotation = Quaternion.Slerp(startRot, targetRot, k);

            yield return null;
        }

        t.localPosition = targetPos;
        t.localRotation = targetRot;
    }
}

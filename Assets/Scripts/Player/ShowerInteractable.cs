using System.Collections;
using UnityEngine;

public class ShowerInteractable : Interactable
{
    [Header("Camera Focus")]
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private float cameraMoveTime = 0.7f;

    [Header("Shower Duration")]
    [SerializeField] private float showerDuration = 8f;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    [Header("Water")]
    [SerializeField] private ParticleSystem[] waterParticles;
    [SerializeField] private AudioSource[] waterSounds;

    private bool isShowering;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        if (!isShowering && (!countAsTask || taskDone))
            StartCoroutine(ShowerSequence());

        return taskDone;
    }

    private IEnumerator ShowerSequence()
    {
        isShowering = true;

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

        SetEffects(waterParticles, true);
        SetEffects(waterSounds, true);

        yield return new WaitForSeconds(showerDuration);

        SetEffects(waterParticles, false);
        SetEffects(waterSounds, false);

        if (cam != null && cameraPoint != null)
        {
            yield return MoveCamera(cam.transform, camPos, camRot, cameraMoveTime);

            if (player != null)
                player.enabled = true;
        }

        isShowering = false;
    }

    private void SetEffects(ParticleSystem[] systems, bool on)
    {
        if (systems == null)
            return;

        foreach (ParticleSystem ps in systems)
        {
            if (ps == null)
                continue;

            if (on)
            {
                if (!ps.gameObject.activeSelf)
                    ps.gameObject.SetActive(true);

                ps.Play();
            }
            else
            {
                ps.Stop();

                if (ps.gameObject.activeSelf)
                    ps.gameObject.SetActive(false);
            }
        }
    }

    private void SetEffects(AudioSource[] sources, bool on)
    {
        if (sources == null)
            return;

        foreach (AudioSource src in sources)
        {
            if (src == null)
                continue;

            if (on)
                src.Play();
            else
                src.Stop();
        }
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

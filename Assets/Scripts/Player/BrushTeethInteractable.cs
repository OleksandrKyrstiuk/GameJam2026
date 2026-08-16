using System.Collections;
using UnityEngine;

public class BrushTeethInteractable : Interactable
{
    [Header("Camera Focus")]
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private float cameraMoveTime = 0.6f;

    [Header("Brush")]
    [SerializeField] private Transform brush;
    [SerializeField] private Vector3 brushUpOffset = new Vector3(0f, 0.35f, 0f);
    [SerializeField] private float brushRiseTime = 0.5f;
    [SerializeField] private float brushReturnTime = 0.4f;

    [Header("Brushing Motion")]
    [SerializeField] private Vector3 brushSideOffset = new Vector3(0.12f, 0f, 0f);
    [SerializeField] private float strokeTime = 0.25f;
    [SerializeField] private int strokeCount = 6;

    [Header("Task")]
    [SerializeField] private bool countAsTask = true;

    [Header("Particles (water etc.)")]
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private AudioSource[] loopSounds;

    private bool isBrushing;

    public override bool Interact()
    {
        bool taskDone = false;

        if (countAsTask)
            taskDone = base.Interact();

        if (!isBrushing && (!countAsTask || taskDone))
            StartCoroutine(BrushSequence());

        return taskDone;
    }

    private IEnumerator BrushSequence()
    {
        isBrushing = true;

        SetEffects(true);

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

        if (brush != null)
        {
            Vector3 startPos = brush.localPosition;
            Vector3 upPos = startPos + brushUpOffset;

            yield return MoveBrush(brush, startPos, upPos, brushRiseTime);

            for (int i = 0; i < strokeCount; i++)
            {
                Vector3 side = (i % 2 == 0) ? upPos + brushSideOffset : upPos - brushSideOffset;

                yield return MoveBrush(brush, brush.localPosition, side, strokeTime);
            }

            yield return MoveBrush(brush, brush.localPosition, startPos, brushReturnTime);
        }

        if (cam != null && cameraPoint != null)
        {
            yield return MoveCamera(cam.transform, camPos, camRot, cameraMoveTime);

            if (player != null)
                player.enabled = true;
        }

        SetEffects(false);

        isBrushing = false;
    }

    private void SetEffects(bool on)
    {
        if (particles != null)
        {
            foreach (ParticleSystem ps in particles)
            {
                if (ps == null)
                    continue;

                if (on)
                    ps.Play();
                else
                    ps.Stop();
            }
        }

        if (loopSounds != null)
        {
            foreach (AudioSource src in loopSounds)
            {
                if (src == null)
                    continue;

                if (on)
                    src.Play();
                else
                    src.Stop();
            }
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

    private IEnumerator MoveBrush(Transform t, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            t.localPosition = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, elapsed / duration));

            yield return null;
        }

        t.localPosition = to;
    }
}

using UnityEngine;

public class AmbientAudio : MonoBehaviour
{
    [SerializeField] private AudioSource ambientSource;

    [Header("Random One-Shots (optional)")]
    [SerializeField] private AudioSource oneShotSource;
    [SerializeField] private AudioClip[] randomClips;
    [SerializeField] private float minDelay = 8f;
    [SerializeField] private float maxDelay = 25f;

    private float nextRandomTime;

    private void Start()
    {
        if (ambientSource != null)
        {
            ambientSource.loop = true;

            if (!ambientSource.isPlaying)
                ambientSource.Play();
        }

        nextRandomTime = Time.time + Random.Range(minDelay, maxDelay);
    }

    private void Update()
    {
        if (randomClips == null || randomClips.Length == 0 || oneShotSource == null)
            return;

        if (Time.time >= nextRandomTime)
        {
            oneShotSource.PlayOneShot(randomClips[Random.Range(0, randomClips.Length)]);

            nextRandomTime = Time.time + Random.Range(minDelay, maxDelay);
        }
    }
}

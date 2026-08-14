using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips;

    [Header("Timing")]
    [SerializeField] private float stepInterval = 0.5f;

    private CharacterController controller;
    private float stepTimer;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f || audioSource == null)
            return;

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        if (controller.isGrounded && horizontalVelocity.sqrMagnitude > 0.1f)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                stepTimer = 0f;
                PlayStep();
            }
        }
        else
        {
            stepTimer = stepInterval * 0.5f;
        }
    }

    private void PlayStep()
    {
        if (footstepClips == null || footstepClips.Length == 0)
        {
            Debug.LogWarning("PlayerFootsteps: no footstep clips assigned!", this);
            enabled = false;
            return;
        }

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        if (clip == null)
            return;

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clip);
    }
}

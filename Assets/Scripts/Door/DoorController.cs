using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    private bool playerInZone;
    private bool doorOpened;

    private Animation doorAnim;
    private BoxCollider doorCollider;

    private void Start()
    {
        doorAnim = transform.parent.gameObject.GetComponent<Animation>();
        doorCollider = transform.parent.gameObject.GetComponent<BoxCollider>();

        if (audioSource == null)
            audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (Input.GetKeyDown(KeyCode.E) && playerInZone && !doorAnim.isPlaying)
        {
            doorOpened = !doorOpened;

            doorAnim.Play(doorOpened ? "Door_Open" : "Door_Close");

            PlaySound(doorOpened ? openClip : closeClip);

            if (doorCollider != null)
                doorCollider.enabled = !doorOpened;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}

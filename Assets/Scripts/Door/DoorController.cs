using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode doorKey = KeyCode.F;

    [Header("UI")]
    [SerializeField] private TMP_Text promptText;

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

        UpdatePrompt();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            UpdatePrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            UpdatePrompt();
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        if (Input.GetKeyDown(doorKey) && playerInZone && !doorAnim.isPlaying)
        {
            doorOpened = !doorOpened;

            doorAnim.Play(doorOpened ? "Door_Open" : "Door_Close");

            PlaySound(doorOpened ? openClip : closeClip);

            if (doorCollider != null)
                doorCollider.enabled = !doorOpened;

            UpdatePrompt();
        }
    }

    private void UpdatePrompt()
    {
        if (promptText == null)
            return;

        if (!playerInZone)
        {
            promptText.text = "";
            return;
        }

        promptText.text = $"[{doorKey}] {(doorOpened ? "Закрити двері" : "Відкрити двері")}";
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}

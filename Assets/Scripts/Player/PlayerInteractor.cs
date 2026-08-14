using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactMask = ~0;

    [Header("UI")]
    [SerializeField] private TMP_Text promptText;

    private Transform cam;
    private Interactable currentTarget;

    private void Start()
    {
        cam = Camera.main != null ? Camera.main.transform : null;

        if (promptText != null)
            promptText.text = "";
    }

    private void Update()
    {
        if (Time.timeScale == 0f || cam == null)
            return;

        FindTarget();
        UpdatePrompt();

        if (currentTarget != null && Input.GetKeyDown(interactKey))
            currentTarget.Interact();
    }

    private void FindTarget()
    {
        currentTarget = null;

        Ray ray = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask))
        {
            currentTarget = hit.collider.GetComponentInParent<Interactable>();
        }
    }

    private void UpdatePrompt()
    {
        if (promptText == null)
            return;

        if (currentTarget != null)
            promptText.text = $"[{interactKey}] {currentTarget.PromptText}";
        else
            promptText.text = "";
    }
}

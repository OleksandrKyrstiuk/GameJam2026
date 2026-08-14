using TMPro;
using UnityEngine;

public class StopwatchTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private bool startOnAwake = true;

    private float elapsed;
    private bool isRunning;

    public float Elapsed => elapsed;

    private void Start()
    {
        isRunning = startOnAwake;
        UpdateDisplay();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        elapsed += Time.deltaTime;
        UpdateDisplay();
    }

    public void StartTimer() => isRunning = true;

    public void StopTimer() => isRunning = false;

    public void ResetTimer()
    {
        elapsed = 0f;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (timeText == null)
            return;

        int minutes = (int)(elapsed / 60f);
        float seconds = elapsed - minutes * 60f;

        timeText.text = $"{minutes:00}:{seconds:00.00}";
    }
}

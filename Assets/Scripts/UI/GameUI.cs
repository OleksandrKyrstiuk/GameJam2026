using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameUI : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Tasks")]
    [SerializeField] private GameObject tasksPanel;

    private bool isPaused;
    private bool musicMuted;
    private bool soundsMuted;

    private void Start()
    {
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void OpenPause()
    {
        isPaused = true;
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ToggleMusic()
    {
        musicMuted = !musicMuted;

        audioMixer.SetFloat("MusicVolume", musicMuted ? -80f : 0f);
    }

    public void ToggleSounds()
    {
        soundsMuted = !soundsMuted;

        audioMixer.SetFloat("SFXVolume", soundsMuted ? -80f : 0f);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void OpenTasks()
    {
        tasksPanel.SetActive(true);
    }

    public void CloseTasks()
    {
        tasksPanel.SetActive(false);
    }
}
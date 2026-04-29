using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EscapeDrivingSchool : MonoBehaviour
{
    public static EscapeDrivingSchool Instance { get; private set; }

    public GameObject pausePanel;
    public GameObject hudCanvas;

    private bool isGamePaused;

    public static bool IsUIOpen => Instance != null && Instance.isGamePaused;

    public static void SetUIOpen(bool value)
    {
        if (Instance != null)
            Instance.isGamePaused = value;
    }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void Start()
    {
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause(!isGamePaused);

        if (!isGamePaused && !IsUIOpen && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Cursor.lockState == CursorLockMode.Locked && isGamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void TogglePause(bool paused)
    {
        isGamePaused = paused;

        pausePanel.SetActive(paused);

        if (hudCanvas != null)
            hudCanvas.SetActive(!paused);

        Time.timeScale = paused ? 0f : 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnResumeClicked()
    {
        TogglePause(false);
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
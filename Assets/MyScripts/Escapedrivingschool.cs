using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EscapeDrivingSchool : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject hudCanvas;

    private bool isGamePaused;
    private bool isHandlingClick;

    public static bool isUIOpen = false;

    void Start()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        isHandlingClick = false;

        pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isUIOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(!isGamePaused);
        }

        if (!isGamePaused && !isUIOpen && Input.GetMouseButtonDown(0))
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

        isHandlingClick = false;
    }

    void TogglePause(bool paused)
    {
        isGamePaused = paused;
        isUIOpen = paused;

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
        if (isHandlingClick) return;
        isHandlingClick = true;
        TogglePause(false);
    }

    public void OnRestartClicked()
    {
        if (isHandlingClick) return;
        isHandlingClick = true;

        isUIOpen = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuClicked()
    {
        if (isHandlingClick) return;
        isHandlingClick = true;

        isUIOpen = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Butans : MonoBehaviour
{
    public GameObject panel;
    public GameObject gameUI;
    private bool isPaused;

    void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause(!isPaused);
        }

        if (!isPaused && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Cursor.lockState == CursorLockMode.Locked && isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void SetPause(bool pause)
    {
        isPaused = pause;
        panel.SetActive(pause);

        if (gameUI != null)
            gameUI.SetActive(!pause);

        Time.timeScale = pause ? 0f : 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void ResumeGame()
    {
        SetPause(false);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeScene()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}
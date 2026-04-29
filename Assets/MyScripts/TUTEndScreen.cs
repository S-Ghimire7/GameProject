using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class TUTEndScreen : MonoBehaviour
{
    public GameObject panel;

    private bool canClick = false;

    void Start()
    {
        panel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("vehicle"))
        {
            StartCoroutine(ShowPanel());
        }
    }

    IEnumerator ShowPanel()
    {
        panel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        EscapeDrivingSchool.SetUIOpen(true);

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        yield return null;

        canClick = true;
    }

    public void fr()
    {
        if (!canClick) return;

        EscapeDrivingSchool.SetUIOpen(false);
        Time.timeScale = 1f;

        SceneManager.LoadScene("City");
    }

    public void mm()
    {
        if (!canClick) return;

        EscapeDrivingSchool.SetUIOpen(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }
}
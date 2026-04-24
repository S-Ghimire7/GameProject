using System.Collections;
using UnityEngine;

public class finalmenu : MonoBehaviour
{
    public GameObject menuUI;
    public GameObject[] otherCanvases;

    private bool hasTriggered = false;
    private bool isMenuOpen = false;

    void Start()
    {
        if (menuUI != null)
            menuUI.SetActive(false);

        StartCoroutine(SetCanvases(true));

        LockCursor();
    }

    void Update()
    {
        if (isMenuOpen)
        {
            UnlockCursor();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("vehicle")) return;

        hasTriggered = true;
        StartCoroutine(OpenMenu());
    }

    IEnumerator OpenMenu()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(SetCanvases(false));

        if (menuUI != null)
            menuUI.SetActive(true);

        isMenuOpen = true;

        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        StartCoroutine(CloseMenuSafe());
    }

    IEnumerator CloseMenuSafe()
    {
        yield return null;

        if (menuUI != null)
            menuUI.SetActive(false);

        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(SetCanvases(true));

        isMenuOpen = false;
        hasTriggered = false;

        Time.timeScale = 1f;

        LockCursor();
    }

    IEnumerator SetCanvases(bool state)
    {
        yield return null;

        if (otherCanvases == null)
            yield break;

        foreach (GameObject c in otherCanvases)
        {
            if (c != null)
                c.SetActive(state);
        }
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
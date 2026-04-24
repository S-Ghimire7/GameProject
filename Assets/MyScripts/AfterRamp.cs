using System.Collections;
using UnityEngine;
using TMPro;

public class AfterRamp : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.05f;

    private Coroutine currentRoutine;
    private bool triggered;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("vehicle"))
        {
            triggered = true;

            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentRoutine = StartCoroutine(TriggerSequence());
        }
    }

    IEnumerator TriggerSequence()
    {
        yield return StartCoroutine(TypeText("Great Job! Now, Follow the Guiding Arrows to the next obstacle."));
        yield return new WaitForSeconds(2f);

        textUI.text = "";
    }

    IEnumerator TypeText(string message)
    {
        if (textUI == null)
        {
            Debug.LogWarning("TextUI is not assigned!");
            yield break;
        }

        textUI.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            textUI.text += message[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
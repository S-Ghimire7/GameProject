using System.Collections;
using UnityEngine;
using TMPro;

public class AfterRamp : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.05f;

    private bool hasTriggered = false;
    private Coroutine currentRoutine;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (hasTriggered)
        {
            Debug.Log("Trigger ignored: already triggered once.");
            return;
        }

        if (other.CompareTag("vehicle"))
        {
            Debug.Log("Vehicle detected - trigger sequence started.");
            hasTriggered = true;

            if (currentRoutine != null)
                StopCoroutine(currentRoutine);

            currentRoutine = StartCoroutine(TriggerSequence());
        }
        else
        {
            Debug.Log("Object entered trigger but is NOT Vehicle: " + other.tag);
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

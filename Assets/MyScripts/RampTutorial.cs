using System.Collections;
using UnityEngine;
using TMPro;

public class RampTutorial : MonoBehaviour
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
        yield return StartCoroutine(TypeText("Now, I want you to slowly align the car with the ramp."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("This is your first obstacle and it will teach you brake control."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("When you get inside the Box marked with 'X', You will have to brake both while going up and down."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("Now, Give it a try yourself!"));
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
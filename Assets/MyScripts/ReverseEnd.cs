using UnityEngine;
using TMPro;
using System.Collections;

public class ReverseEnd : MonoBehaviour
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
        yield return StartCoroutine(TypeText("Please stop the vehicle!"));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("This is the final part of the tutorial "));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("As I mentioned earler, There are 7 gears in our car in total."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("One of the gears is reverse."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("I am sure you saw a parking space when you got here if you didn't, you can turn around and look."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("Your final task is to reverse your car inside the parking area."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("To do that, first you downshift to Reverse Gear and drive the car in reverse accordingly."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("You can look at the side mirrors or turn completely around according to your choice."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("Congratulations! now you know how driving a car works."));
        yield return new WaitForSeconds(1f);

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
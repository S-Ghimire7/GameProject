using UnityEngine;
using TMPro;
using System.Collections;

public class AfterE : MonoBehaviour
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
        yield return StartCoroutine(TypeText("Great Job! Please Stop the vehicle once again."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("Now, Let's learn about gear shifting and engine braking."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("There are 7 gears in our car in total. They are Reverse, Neutral, 1, 2, 3, 4 and 5."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("As we learned previously, We can shift the gear up by pressing clutch 'C' and 'LShift'. This is called upshifting."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("But another thing we can do is shift the gear down by pressing clutch 'C' and 'LCtrl'. This is called downshifting."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("This brings us to engine braking. Engine braking is simply slowing your vehicle down using the engine."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("We can do that by simply downshifting while at high speed."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("Now I want you to give it a try. Upshift first, speed up, and then downshift at high speed."));
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
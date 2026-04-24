using System.Collections;
using UnityEngine;
using TMPro;

public class ETUT : MonoBehaviour
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
        yield return StartCoroutine(TypeText("Please Stop Your Car!"));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("This next obstacle is called the 8"));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("This obstacle will teach you turning in a harsh but understandable way."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("Be Careful not to hit the cones or go out of bounds."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("But even if you do, You can press the 'Esc' key on your keyboard and restart."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("Now, Go ahead and give it a try."));
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
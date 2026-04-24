using System.Collections;
using UnityEngine;
using TMPro;

public class RampTutorial : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.05f;
    public float stopTime = 20f;

    private Coroutine routine;
    private bool triggered;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("vehicle")) return;

        if (triggered) return;
        triggered = true;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            StartCoroutine(StopVehicle(rb));
        }

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(TextSequence());
    }

    IEnumerator StopVehicle(Rigidbody rb)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        yield return new WaitForSeconds(stopTime);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    IEnumerator TextSequence()
    {
        yield return TypeText("This is your first obstacle. It will teach you brake control.");
        yield return new WaitForSeconds(2f);

        yield return TypeText("Slowly align your car with the ramp.");
        yield return new WaitForSeconds(2f);

        yield return TypeText("Stop inside the marked box while going up and down.");
        yield return new WaitForSeconds(2f);

        yield return TypeText("Now try it yourself!");
        yield return new WaitForSeconds(2f);

        textUI.text = "";
    }

    IEnumerator TypeText(string message)
    {
        textUI.text = "";

        foreach (char c in message)
        {
            textUI.text += c;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                textUI.text = message;
                yield break;
            }

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
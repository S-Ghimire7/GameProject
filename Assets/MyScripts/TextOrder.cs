using System.Collections;
using UnityEngine;
using TMPro;

public class TextOrder : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.05f;

    private bool introFinished = false;
    private bool secondSequenceStarted = false;
    private bool thirdSequenceStarted = false;

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    void Update()
    {
        if (introFinished && !secondSequenceStarted && Input.GetKeyDown(KeyCode.CapsLock))
        {
            secondSequenceStarted = true;
            StartCoroutine(ButtonSequence());
        }

        if (secondSequenceStarted && !thirdSequenceStarted &&
            Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.LeftShift))
        {
            thirdSequenceStarted = true;
            StartCoroutine(ThirdSequence());
        }
    }

    IEnumerator PlaySequence()
    {
        yield return StartCoroutine(TypeText("Welcome to the tutorial section of the Game."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("This is where you will learn how to drive the car."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("Firstly, I want you to start the car."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("For that, you will need to press 'CapsLock' on your keyboard."));

        introFinished = true;
    }

    IEnumerator ButtonSequence()
    {
        yield return StartCoroutine(TypeText("Great! You have successfully started the car."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("Now, just like a real car, for it to move we need to put it in the first gear,"));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("To do that, We need to press the clutch 'C' and shift the gear up using 'LShift' "));
    }

    IEnumerator ThirdSequence()
    {
        yield return StartCoroutine(TypeText("Perfect! You shifted into the first gear."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("As you can see everything that is going around can be seen around you,"));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("You have your speed, When your clutch is pressed, As well as the side mirrors."));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypeText("To accelerate the car you press 'W' and to slow down or stop the car you press 'S' and 'SpaceBar' respectively."));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("You can turn the car using 'A' to go left and 'D' to go right. "));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("But, always remember to indicate where you are going for which you can use 'Q' and 'E' for left and right indicator respectively. "));
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypeText("Now, Please Follow the Arrows which will lead to your first obstacle."));
        yield return new WaitForSeconds(2f);

        textUI.text = "";
    }

    IEnumerator TypeText(string message)
    {
        textUI.text = "";

        for (int i = 0; i < message.Length; i++)
        {
            textUI.text += message[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
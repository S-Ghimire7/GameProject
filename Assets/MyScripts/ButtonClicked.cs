using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClicked : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clickSound;

    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (audioSource && clickSound)
            audioSource.PlayOneShot(clickSound);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(PlaySound);
    }
}
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ButtonSlide : MonoBehaviour
{
    public Vector2 startPosition;
    public Vector2 targetPosition;
    public float duration = 0.8f;

    public AudioSource audioSource;
    public AudioClip slideSound;

    RectTransform rectTransform;
    float elapsedTime;
    bool isAnimating;
    bool hasPlayedSound;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        rectTransform.anchoredPosition = startPosition;
        StartSlide();
    }

    public void StartSlide()
    {
        elapsedTime = 0f;
        isAnimating = true;
        hasPlayedSound = false;
    }

    void Update()
    {
        if (!isAnimating) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        t = t * t * (3f - 2f * t);

        rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

        if (!hasPlayedSound && t >= 0.92f)
        {
            if (audioSource && slideSound)
                audioSource.PlayOneShot(slideSound);

            hasPlayedSound = true;
        }

        if (t >= 1f)
        {
            rectTransform.anchoredPosition = targetPosition;
            isAnimating = false;
        }
    }
}
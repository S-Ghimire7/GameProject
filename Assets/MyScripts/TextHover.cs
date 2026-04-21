using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text text;
    public float hoverScale = 1.15f;
    public float smoothSpeed = 10f;

    Vector3 originalScale;
    Vector3 targetScale;

    void Awake()
    {
        if (!text)
            text = GetComponentInChildren<TMP_Text>();

        originalScale = text.transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        text.transform.localScale = Vector3.Lerp(
            text.transform.localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
        text.fontStyle |= FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        text.fontStyle &= ~FontStyles.Underline;
    }
}
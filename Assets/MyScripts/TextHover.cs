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
        if (text == null)
            text = GetComponent<TMP_Text>();

        if (text == null)
            text = GetComponentInChildren<TMP_Text>();

        originalScale = transform.localScale;
        targetScale = originalScale;
    }

   void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * smoothSpeed
        );
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;

        if (text != null)
            text.fontStyle |= FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;

        if (text != null)
            text.fontStyle &= ~FontStyles.Underline;
    }
}
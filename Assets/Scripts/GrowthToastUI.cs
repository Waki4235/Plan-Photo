using System.Collections;
using TMPro;
using UnityEngine;

public class GrowthToastUI : MonoBehaviour
{
    [SerializeField] private TMP_Text toastText;
    [SerializeField] private string message = "GROWN UP !";

    [Header("Animation")]
    [SerializeField] private float fadeIn = 0.08f;
    [SerializeField] private float hold = 0.35f;
    [SerializeField] private float fadeOut = 0.18f;
    [SerializeField] private float moveUp = 25f;

    private Coroutine co;

    private void Reset()
    {
        toastText = GetComponent<TMP_Text>();
    }

    private void Awake()
    {
        if (toastText == null) toastText = GetComponent<TMP_Text>();
        SetAlpha(0f);
        //gameObject.SetActive(false);
    }

    public void Show()
    {
        if (toastText == null) return;

        if (co != null) StopCoroutine(co);
        co = StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        //gameObject.SetActive(true);

        toastText.text = message;

        RectTransform rt = toastText.rectTransform;
        Vector2 basePos = rt.anchoredPosition;
        Vector2 peakPos = basePos + Vector2.up * moveUp;

        // start
        rt.anchoredPosition = basePos;
        SetAlpha(0f);

        // fade in + move up
        for (float t = 0; t < fadeIn; t += Time.deltaTime)
        {
            float k = t / Mathf.Max(0.001f, fadeIn);
            SetAlpha(k);
            rt.anchoredPosition = Vector2.Lerp(basePos, peakPos, EaseOutCubic(k));
            yield return null;
        }
        SetAlpha(1f);
        rt.anchoredPosition = peakPos;

        // hold
        float holdEnd = Time.time + hold;
        while (Time.time < holdEnd) yield return null;

        // fade out
        for (float t = 0; t < fadeOut; t += Time.deltaTime)
        {
            float k = t / Mathf.Max(0.001f, fadeOut);
            SetAlpha(1f - k);
            yield return null;
        }

        SetAlpha(0f);
        rt.anchoredPosition = basePos;
        //gameObject.SetActive(false);
        co = null;
    }

    private void SetAlpha(float a)
    {
        Color c = toastText.color;
        c.a = a;
        toastText.color = c;
    }

    private float EaseOutCubic(float x)
    {
        return 1f - Mathf.Pow(1f - x, 3f);
    }
}

using System.Collections;
using UnityEngine;

public class GrowthDisplay : MonoBehaviour
{
    [Header("Stage Models (index = stage)")]
    [SerializeField] private GameObject[] stages;

    [Header("Animation")]
    [SerializeField] private bool playAnimationOnChange = true;
    [SerializeField] private float popScale = 1.12f;     // 1.05〜1.2推奨
    [SerializeField] private float popDuration = 0.18f;  // 0.12〜0.25推奨
    [SerializeField] private float rotateDegrees = 10f;  // 0なら回転なし

    private int currentStage = 0;
    private Coroutine animCo;

    private void Awake()
    {
        ApplyStage(currentStage, animate: false);
    }

    public void SetStage(int stageIndex)
    {
        int clamped = Mathf.Clamp(stageIndex, 0, stages.Length - 1);
        bool changed = clamped != currentStage;

        currentStage = clamped;
        ApplyStage(currentStage, animate: changed && playAnimationOnChange);
    }

    private void ApplyStage(int stageIndex, bool animate)
    {
        if (stages == null || stages.Length == 0) return;

        GameObject active = null;

        for (int i = 0; i < stages.Length; i++)
        {
            var go = stages[i];
            if (go == null) continue;

            bool on = (i == stageIndex);
            go.SetActive(on);

            if (on) active = go;
        }

        if (animate && active != null)
        {
            if (animCo != null) StopCoroutine(animCo);
            animCo = StartCoroutine(PopAnim(active.transform));
        }
    }

    private IEnumerator PopAnim(Transform target)
    {
        Vector3 baseScale = target.localScale;
        Quaternion baseRot = target.localRotation;

        Vector3 peakScale = baseScale * popScale;
        Quaternion peakRot = baseRot * Quaternion.Euler(0f, rotateDegrees, 0f);

        // 0→1（膨らむ）
        float half = Mathf.Max(0.001f, popDuration * 0.5f);
        for (float t = 0; t < half; t += Time.deltaTime)
        {
            float k = EaseOutBack(t / half);
            target.localScale = Vector3.LerpUnclamped(baseScale, peakScale, k);
            target.localRotation = Quaternion.Slerp(baseRot, peakRot, k);
            yield return null;
        }

        // 1→0（戻る）
        for (float t = 0; t < half; t += Time.deltaTime)
        {
            float k = EaseOutCubic(t / half);
            target.localScale = Vector3.LerpUnclamped(peakScale, baseScale, k);
            target.localRotation = Quaternion.Slerp(peakRot, baseRot, k);
            yield return null;
        }

        target.localScale = baseScale;
        target.localRotation = baseRot;
        animCo = null;
    }

    // ちょい弾む
    private float EaseOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);
    }

    // なめらか
    private float EaseOutCubic(float x)
    {
        return 1f - Mathf.Pow(1f - x, 3f);
    }
}

using UnityEngine;

public class GrowthManager : MonoBehaviour
{
    [SerializeField] private GrowthDisplay display;

    [Header("Progress (debug / later: upload count)")]
    [SerializeField] private int uploadCount = 0;

    [Header("Stage thresholds (inclusive)")]
    // 例: stage0=0〜0, stage1=1〜2, stage2=3〜∞
    [SerializeField] private int stage1At = 1;
    [SerializeField] private int stage2At = 3;

    private void Awake()
    {
        if (display == null) display = FindFirstObjectByType<GrowthDisplay>();
        ApplyByCount();
    }

    /// <summary>
    /// 画像アップロード成功時に呼ぶ予定の関数
    /// </summary>
    public void NotifyUploaded()
    {
        uploadCount++;
        ApplyByCount();
    }

    /// <summary>
    /// デバッグ用：カウントを手で増やす
    /// </summary>
    [ContextMenu("Debug/Add Upload")]
    public void DebugAddUpload()
    {
        NotifyUploaded();
    }

    private void ApplyByCount()
    {
        if (display == null) return;

        int stage = 0;
        if (uploadCount >= stage2At) stage = 2;
        else if (uploadCount >= stage1At) stage = 1;

        display.SetStage(stage);
    }
}

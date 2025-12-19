using UnityEngine;

public class GrowthDisplay : MonoBehaviour
{
    [Header("Stage Models (index = stage)")]
    [SerializeField] private GameObject[] stages;

    [Header("Debug")]
    [SerializeField] private int currentStage = 0;

    private void Awake()
    {
        ApplyStage(currentStage);
    }

    /// <summary>
    /// Show only the specified stage index.
    /// </summary>
    public void SetStage(int stageIndex)
    {
        currentStage = Mathf.Clamp(stageIndex, 0, stages.Length - 1);
        ApplyStage(currentStage);
    }

    private void ApplyStage(int stageIndex)
    {
        if (stages == null || stages.Length == 0) return;

        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i] == null) continue;
            stages[i].SetActive(i == stageIndex);
        }
    }

}

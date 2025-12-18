using UnityEngine;

public class PlanListUI : MonoBehaviour
{
    [Header("References")]
    public Transform contentParent;          // ScrollView / Content
    public PlanItemUI planItemPrefab;         // PlanItem.prefab

    // プランを1件追加
    public void AddItem(string planName)
    {
        PlanItemUI item =
            Instantiate(planItemPrefab, contentParent);

        // 未アップロード用の画像（Prefab側に設定済み想定）
        item.Setup(planName, null);
    }
}

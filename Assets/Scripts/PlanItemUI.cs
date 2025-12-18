using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlanItemUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text planNameText;
    public Image thumbnailImage;

    // 初期表示用
    public void Setup(string planName, Sprite thumbnail)
    {

        Debug.Log("PlanItemUI.Setup called: " + planName);

        planNameText.text = planName;

        if (thumbnail != null)
        {
            thumbnailImage.sprite = thumbnail;
        }
    }

    // 写真がアップロードされたとき用
    public void SetThumbnail(Sprite thumbnail)
    {
        if (thumbnail != null)
        {
            thumbnailImage.sprite = thumbnail;
        }
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlanItemUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text planNameText;
    public Image thumbnailImage;

    [Header("Upload Button (Optional)")]
    public Button uploadButton;

    [Header("Growth (Optional)")]
    [SerializeField] private GrowthManager growthManager;

    private string _planName;

    public void Setup(string planName, Sprite thumbnail)
    {
        Debug.Log("PlanItemUI.Setup called: " + planName);

        _planName = planName;

        if (planNameText != null)
            planNameText.text = planName;

        if (thumbnail != null && thumbnailImage != null)
            thumbnailImage.sprite = thumbnail;

        if (uploadButton != null)
        {
            uploadButton.onClick.RemoveAllListeners();
            uploadButton.onClick.AddListener(OnClickUpload);
        }

        if (growthManager == null)
            growthManager = FindFirstObjectByType<GrowthManager>();
    }

    public void SetThumbnail(Sprite thumbnail)
    {
        if (thumbnail != null && thumbnailImage != null)
        {
            thumbnailImage.sprite = thumbnail;
            thumbnailImage.preserveAspect = true;
        }
    }

    private void OnClickUpload()
    {
#if UNITY_EDITOR
        string path = EditorUtility.OpenFilePanel("Select photo", "", "png,jpg,jpeg");

        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("Upload canceled.");
            return;
        }

        if (!TryLoadImageAsSprite(path, out Sprite sprite) || sprite == null)
        {
            Debug.LogWarning("Failed to load image: " + path);
            return;
        }

        SetThumbnail(sprite);
        SaveThumbnailPath(path);

        if (growthManager != null)
            growthManager.NotifyUploaded();

        Debug.Log($"Thumbnail updated for plan '{_planName}' : {Path.GetFileName(path)}");
#else
        Debug.LogWarning("File picker is Editor-only without SFB. Add SFB (StandaloneFileBrowser) if you need this in builds.");
#endif
    }

    private bool TryLoadImageAsSprite(string filePath, out Sprite sprite)
    {
        sprite = null;

        try
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            byte[] bytes = File.ReadAllBytes(filePath);

            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (!tex.LoadImage(bytes))
                return false;

            sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("TryLoadImageAsSprite error: " + e.Message);
            return false;
        }
    }

    private void SaveThumbnailPath(string path)
    {
        if (string.IsNullOrEmpty(_planName)) return;

        PlayerPrefs.SetString($"PlanThumbPath::{_planName}", path);
        PlayerPrefs.Save();
    }

    public void LoadThumbnailIfSaved()
    {
        if (string.IsNullOrEmpty(_planName)) return;

        string key = $"PlanThumbPath::{_planName}";
        if (!PlayerPrefs.HasKey(key)) return;

        string path = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return;

        if (TryLoadImageAsSprite(path, out Sprite sprite) && sprite != null)
            SetThumbnail(sprite);
    }
}

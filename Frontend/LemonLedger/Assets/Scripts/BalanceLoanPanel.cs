using UnityEngine;
using UnityEngine.UI;

public class BalanceLoanPanel : MonoBehaviour
{
    [Header("Panels (must have an Image component)")]
    [Tooltip("Panel to show when Balance ≥ Loan")]
    public GameObject enoughPanel;

    [Tooltip("Panel to show when Balance < Loan")]
    public GameObject insufficientPanel;

    Image _enoughImg;
    Image _insufficientImg;

    void Awake()
    {
        // cache the Image components
        if (enoughPanel != null)
            _enoughImg = enoughPanel.GetComponent<Image>();

        if (insufficientPanel != null)
            _insufficientImg = insufficientPanel.GetComponent<Image>();

        // start fully transparent
        SetAlpha(_enoughImg, 0f);
        SetAlpha(_insufficientImg, 0f);
    }

    /// <summary>
    /// Call this (e.g. on your Pay button) to flip which panel is opaque.
    /// </summary>
    public void ShowStatus()
    {
        bool hasEnough = GameData.Balance >= GameData.Loan;

        // if you want to use 0–255 alpha instead, you can do:
        // byte a = hasEnough ? (byte)255 : (byte)0;
        // SetAlpha32(_enoughImg, a);
        // SetAlpha32(_insufficientImg, (byte)(255 - a));

        // using 0–1 floats:
        SetAlpha(_enoughImg, hasEnough ? 1f : 0f);
        SetAlpha(_insufficientImg, hasEnough ? 0f : 1f);
    }

    void SetAlpha(Image img, float alpha)
    {
        if (img == null) return;
        var c = img.color;
        c.a = alpha;         // 0 = transparent, 1 = opaque
        img.color = c;
    }

    // -- Optional: if you really want to think in 0–255 --
    void SetAlpha32(Image img, byte alpha)
    {
        if (img == null) return;
        Color32 c32 = img.color;
        c32.a = alpha;       // 0–255
        img.color = c32;     // implicit conversion back to Color
    }
}

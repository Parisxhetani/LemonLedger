using UnityEngine;
using DG.Tweening;

public class CardAnimator : MonoBehaviour
{
    [Tooltip("RectTransform of the card")]
    public RectTransform card;
    public float slideDuration = 0.4f;
    Vector2 offscreenPos;

    void Awake()
    {
        offscreenPos = card.anchoredPosition + new Vector2(0, -Screen.height);
        card.anchoredPosition = offscreenPos;
    }

    /// <summary>
    /// Slide the card up into view
    /// </summary>
    public void SlideIn()
    {
        card
          .DOAnchorPosY(0, slideDuration)
          .SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// Slide the card back down
    /// </summary>
    public void SlideOut()
    {
        card
          .DOAnchorPos(offscreenPos, slideDuration * 0.8f)
          .SetEase(Ease.InCubic);
    }
}

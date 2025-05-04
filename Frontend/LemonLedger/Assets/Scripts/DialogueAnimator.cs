// DialogueAnimator.cs
using UnityEngine;
using DG.Tweening;

public class DialogueAnimator : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public RectTransform Comment;   // ← drag your “Comment” UI Image’s RectTransform here

    [Header("Tween Settings")]
    public float popDuration = 0.5f;

    void Awake()
    {
        DOTween.Init();                // initialize DOTween once
        Comment.localScale = Vector3.zero;
    }

    public void PopIn()
    {
        Comment
          .DOScale(Vector3.one, popDuration)
          .SetEase(Ease.OutBack);
    }

    public void PunchOnNextLine()
    {
        Comment
          .DOPunchScale(Vector3.one * 0.1f, 0.2f, 10)
          .SetEase(Ease.OutQuad);
    }
}

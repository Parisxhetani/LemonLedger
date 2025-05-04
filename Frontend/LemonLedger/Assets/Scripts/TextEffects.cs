using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextEffects : MonoBehaviour
{
    public TMP_Text text;
    public float fadeDuration = 0.3f;
    public float pulseInterval = 1f;

    void Start()
    {
        // continuous pulse
        text
          .DOColor(Color.yellow, pulseInterval)
          .SetLoops(-1, LoopType.Yoyo)
          .SetEase(Ease.InOutSine);
    }

    public void FadeIn()
    {
        text.DOFade(1f, fadeDuration);
    }

    public void FadeOut()
    {
        text.DOFade(0f, fadeDuration);
    }
}

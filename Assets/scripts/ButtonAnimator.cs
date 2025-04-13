using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimator : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void AnimatePress()
    {
        rectTransform.DOKill();

        rectTransform.DOScale(0.9f, 0.1f)
            .OnComplete(() =>
            {
                rectTransform.DOScale(1f, 0.1f);
            });
    }
}

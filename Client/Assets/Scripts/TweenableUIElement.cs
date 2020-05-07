using UnityEngine;
using DG.Tweening;

public class TweenableUIElement : MonoBehaviour
{
    [HideInInspector]
    public RectTransform rectTransform;

    public float time;

    public int insightPosition;
    public int hidePosition;

    public bool isShowing;
    public bool moveX;
    public Ease ease;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ShowPanel()
    {
        if (moveX)
        {
            rectTransform.DOAnchorPosX(insightPosition, time, true).SetEase(ease);
        }
        else
        {
            rectTransform.DOAnchorPosY(insightPosition, time, true).SetEase(ease);
        }
    }

    public void HidePanel()
    {
        if (moveX)
        {
            rectTransform.DOAnchorPosX(hidePosition, time, true).SetEase(ease);
        }
        else
        {
            rectTransform.DOAnchorPosY(hidePosition, time, true).SetEase(ease);
        }
    }

    public void ShowOrHidePanel()
    {
        var pos = isShowing ? hidePosition : insightPosition;

        isShowing = !isShowing;

        if (moveX)
        {
            rectTransform.DOAnchorPosX(pos, time, true).SetEase(ease);
        }
        else
        {
            rectTransform.DOAnchorPosY(pos, time, true).SetEase(ease);
        }
    }
}
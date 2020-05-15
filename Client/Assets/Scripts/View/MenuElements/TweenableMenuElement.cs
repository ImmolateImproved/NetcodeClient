using UnityEngine;
using DG.Tweening;

public class TweenableMenuElement : MenuElement
{
    private RectTransform rectTransform;

    [SerializeField]
    private TweenableMenuElement[] panelToHide;

    [SerializeField]
    private float time;

    [SerializeField]
    private int insightPosition;

    [SerializeField]
    private int hidePosition;

    [SerializeField]
    private bool moveX;

    [SerializeField]
    private Ease ease;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Move(int position)
    {
        if (moveX)
        {
            rectTransform.DOAnchorPosX(position, time, true).SetEase(ease);
        }
        else
        {
            rectTransform.DOAnchorPosY(position, time, true).SetEase(ease);
        }
    }

    public override void Show()
    {
        if (isShowing)
            return;

        foreach (var item in panelToHide)
        {
            item.Hide();
        }

        isShowing = true;

        Move(insightPosition);
    }

    public override void Hide()
    {
        if (!isShowing)
            return;

        isShowing = false;

        Move(hidePosition);
    }
}
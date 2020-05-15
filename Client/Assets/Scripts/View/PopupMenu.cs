using MEC;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenableMenuElement))]
public class PopupMenu : MonoBehaviour
{
    [SerializeField]
    private Text uiText;

    private TweenableMenuElement myTweenable;

    public event Action OnHide = delegate { };

    private void Awake()
    {
        myTweenable = GetComponent<TweenableMenuElement>();
    }

    public void Show(string text)
    {
        uiText.text = text;
        myTweenable.Show();
    }

    public void Show(string text, float time)
    {
        Show(text);
        Timing.CallDelayed(time, Hide);
    }

    public void Hide()
    {
        myTweenable.Hide();
        OnHide();
    }
}

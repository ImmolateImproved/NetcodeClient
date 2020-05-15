using UnityEngine;

public class MenuElement : MonoBehaviour
{
    [SerializeField]
    protected bool isShowing;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowOrHide()
    {
        if (isShowing)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    public void ShowOrHide(bool show)
    {
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}

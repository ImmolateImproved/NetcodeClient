using UnityEngine;

public enum MenuType
{
    Login, Registration, Profile
}

[System.Serializable]
public struct TweenableGroup
{
    public MenuType menuType;
    public MenuElement[] tweenables;
}

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private TweenableGroup[] tweenableGroups;

    [SerializeField]
    private MenuElement[] elementsToHide;

    public void Show(int menuType)
    {
        Show((MenuType)menuType);
    }

    public void Show(MenuType menuType)
    {
        foreach (var item in tweenableGroups)
        {
            foreach (var tweenable in item.tweenables)
            {
                tweenable.ShowOrHide(item.menuType == menuType);
            }
        }

        foreach (var item in elementsToHide)
        {
            item.Hide();
        }
    }
}
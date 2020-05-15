using UnityEngine;

public class RegistrationUI : MonoBehaviour
{
    [SerializeField]
    private MenuController menuController;

    [SerializeField]
    private PopupMenu popupMenu;

    private void OnEnable()
    {
        RegistrationManager.OnRegistrationSucceed += OnRegistrationSucceed;
        popupMenu.OnHide += PopupMenu_OnHide;
    }

    private void OnDisable()
    {
        RegistrationManager.OnRegistrationSucceed -= OnRegistrationSucceed;
        popupMenu.OnHide -= PopupMenu_OnHide;
    }

    private void OnRegistrationSucceed()
    {
        popupMenu.Show("Registration succeed");
    }

    private void PopupMenu_OnHide()
    {
        menuController.Show(MenuType.Login);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class RegistrationUI : MonoBehaviour
{
    private RegistrationManager registrationManager;

    [SerializeField]
    private InputField loginInput, passwordInput;

    [SerializeField]
    private MenuController menuController;

    [SerializeField]
    private PopupMenu popupMenu;

    private void Awake()
    {
        registrationManager = LogicManager.GetLogicComponent<RegistrationManager>();
    }

    private void OnEnable()
    {
        registrationManager.OnRegistrationSucceed += OnRegistrationSucceed;
        popupMenu.OnHide += PopupMenu_OnHide;
    }

    private void OnDisable()
    {
        registrationManager.OnRegistrationSucceed -= OnRegistrationSucceed;
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

    public void Registration()
    {
        registrationManager.Registration(loginInput.text, passwordInput.text);
    }
}
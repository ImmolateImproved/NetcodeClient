using DG.Tweening;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationUI : MonoBehaviour
{
    private Authentication authentication;

    [SerializeField]
    private MenuController menuController;

    [SerializeField]
    private InputField loginInput, passwordInput;

    private void Awake()
    {
        authentication = LogicManager.GetLogicComponent<Authentication>();
    }

    private void OnEnable()
    {
        authentication.OnAuthentification += OnAuthentification;
        authentication.OnTokenNotFound += OnTokenNotFound;
    }

    private void OnDisable()
    {
        authentication.OnAuthentification -= OnAuthentification;
        authentication.OnTokenNotFound -= OnTokenNotFound;
    }

    private void OnTokenNotFound()
    {
        menuController.Show(MenuType.Login);
    }

    private void OnAuthentification(bool status)
    {
        menuController.Show(MenuType.Profile);

        if (!status)
        {
            Debug.Log("ОШИБКА АУТЕНТИФИКАЦИИ");
        }
    }

    public void Login()
    {
        authentication.Login(loginInput.text, passwordInput.text);
    }
}
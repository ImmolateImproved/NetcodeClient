using DG.Tweening;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationUI : MonoBehaviour
{
    [SerializeField]
    private Authentication authentication;

    [SerializeField]
    private MenuController menuController;

    [SerializeField]
    private InputField loginInput, passwordInput;

    private void Start()
    {
        authentication.SetLogin(loginInput.text);
        authentication.SetPassword(passwordInput.text);
    }

    private void OnEnable()
    {
        Authentication.OnAuthentification += OnAuthentification;
        Authentication.OnTokenNotFound += OnTokenNotFound;
    }

    private void OnDisable()
    {
        Authentication.OnAuthentification -= OnAuthentification;
        Authentication.OnTokenNotFound -= OnTokenNotFound;
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
}
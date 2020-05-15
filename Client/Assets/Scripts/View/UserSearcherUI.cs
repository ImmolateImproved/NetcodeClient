using MEC;
using System.Collections.Generic;
using UnityEngine;

public class UserSearcherUI : MonoBehaviour
{
    [SerializeField]
    private User userUI;

    private TweenableMenuElement userMenuElement;

    private void Awake()
    {
        userMenuElement = userUI.GetComponent<TweenableMenuElement>();
    }

    private void OnEnable()
    {
        UserSearcher.OnUserFinded += OnlineManager_OnUserFinded;
    }

    private void OnDisable()
    {
        UserSearcher.OnUserFinded -= OnlineManager_OnUserFinded;
    }

    private void OnlineManager_OnUserFinded(UserData userData)
    {
        userUI.Init(userData);

        userMenuElement.Show();
    }
}
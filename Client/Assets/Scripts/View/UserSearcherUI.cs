using MEC;
using System.Collections.Generic;
using UnityEngine;

public class UserSearcherUI : MonoBehaviour
{
    private UserSearcher userSearcher;

    [SerializeField]
    private User userUI;

    private TweenableMenuElement userMenuElement;

    private void Awake()
    {
        userSearcher = LogicManager.GetLogicComponent<UserSearcher>();
        userMenuElement = userUI.GetComponent<TweenableMenuElement>();
    }

    private void OnEnable()
    {
        userSearcher.OnUserFinded += OnlineManager_OnUserFinded;
    }

    private void OnDisable()
    {
        userSearcher.OnUserFinded -= OnlineManager_OnUserFinded;
    }

    private void OnlineManager_OnUserFinded(UserData userData)
    {
        userUI.Init(userData);

        userMenuElement.Show();
    }
}
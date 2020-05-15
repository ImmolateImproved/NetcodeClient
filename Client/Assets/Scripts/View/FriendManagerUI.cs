using MEC;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendManagerUI : MonoBehaviour
{
    [SerializeField]
    private FriendListManager friendManager;

    [SerializeField]
    private TweenableMenuElement frendsRequestPanel;

    [SerializeField]
    private User userUI;

    private bool ready = true;

    private void OnEnable()
    {
        FriendListManager.OnFriendRequest += FriendsManager_OnFriendRequest;
    }

    private void OnDisable()
    {
        FriendListManager.OnFriendRequest -= FriendsManager_OnFriendRequest;
    }

    private void FriendsManager_OnFriendRequest(UserData userData)
    {
        if (ready)
        {
            userUI.Init(userData);
            frendsRequestPanel.Show();

            ready = false;
        }
    }

    public void AcceptFrend()
    {
        friendManager.AcceptFriend(userUI.userData);
        frendsRequestPanel.Hide();

        ready = true;
    }

    public void DeclineFrend()
    {
        friendManager.DeclineFriend();
        frendsRequestPanel.Hide();

        ready = true;
    }
}
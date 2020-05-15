using MEC;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendManagerUI : MonoBehaviour
{
    private FriendListManager friendListManager;

    [SerializeField]
    private TweenableMenuElement frendsRequestPanel;

    [SerializeField]
    private User userUI;

    private bool ready = true;

    private void Awake()
    {
        friendListManager = LogicManager.GetLogicComponent<FriendListManager>();
    }

    private void OnEnable()
    {
        friendListManager.OnFriendRequest += FriendsManager_OnFriendRequest;
    }

    private void OnDisable()
    {
        friendListManager.OnFriendRequest -= FriendsManager_OnFriendRequest;
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
        friendListManager.AcceptFriend(userUI.userData);
        frendsRequestPanel.Hide();

        ready = true;
    }

    public void DeclineFrend()
    {
        friendListManager.DeclineFriend();
        frendsRequestPanel.Hide();

        ready = true;
    }
}
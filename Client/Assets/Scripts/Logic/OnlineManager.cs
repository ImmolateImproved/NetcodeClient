using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using MEC;

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/OnlineManager")]
public class OnlineManager : Logic
{
    private ProfileManager profileManager;

    private FriendListManager friendsManager;

    [NonSerialized]
    private List<UserData> friendsOnline = new List<UserData>();

    public Dictionary<int, string> IdToNicknameMap { get; private set; } = new Dictionary<int, string>();

    public event Action<List<UserData>> OnlineChanged = delegate { };

    public override void Init()
    {
        profileManager = LogicManager.GetLogicComponent<ProfileManager>();
        friendsManager = LogicManager.GetLogicComponent<FriendListManager>();

        friendsOnline = new List<UserData>();
    }

    public override void MyOnEnable()
    {
        profileManager.OnReciveOnlneFriends += ProfileManager_OnReciveOnlneFriends;
        friendsManager.OnAddToFriends += FriendsManager_OnAddToFriends;
    }

    public override void MyOnDisable()
    {
        profileManager.OnReciveOnlneFriends -= ProfileManager_OnReciveOnlneFriends;
        friendsManager.OnAddToFriends -= FriendsManager_OnAddToFriends;
    }

    private void FriendsManager_OnAddToFriends(UserData userData)
    {
        friendsOnline.Add(userData);
        OnlineChanged(friendsOnline);
    }

    private void ProfileManager_OnReciveOnlneFriends(int[] friendOnlineIDs)
    {
        friendsOnline = friendsManager.GetOnlineFriendsByIDs(friendOnlineIDs);

        OnlineChanged(friendsOnline);

        for (int i = 0; i < friendsOnline.Count; i++)
        {
            var id = friendsOnline[i].id;

            IdToNicknameMap[id] = friendsOnline[i].nick;
        }
    }
}
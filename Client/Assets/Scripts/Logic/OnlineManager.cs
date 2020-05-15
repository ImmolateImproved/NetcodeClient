using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using MEC;

public class OnlineManager : MonoBehaviour
{
    [SerializeField]
    private Socket socket;

    [SerializeField]
    private FriendListManager friendsManager;

    private List<UserData> friendsOnline = new List<UserData>();

    public Dictionary<int, string> IdToNicknameMap { get; private set; } = new Dictionary<int, string>();

    public static event Action<List<UserData>> OnlineChanged = delegate { };

    private void OnEnable()
    {
        FriendListManager.OnAddToFriends += FriendsManager_OnAddToFriends;
        ProfileManager.OnReciveOnlneFriends += ProfileManager_OnReciveOnlneFriends;
    }

    private void OnDisable()
    {
        FriendListManager.OnAddToFriends -= FriendsManager_OnAddToFriends;
        ProfileManager.OnReciveOnlneFriends -= ProfileManager_OnReciveOnlneFriends;
    }

    private void FriendsManager_OnAddToFriends(UserData userData)
    {
        friendsOnline.Add(userData);
        OnlineChanged(friendsOnline);
    }

    private void ProfileManager_OnReciveOnlneFriends(int[] friendOnlineIDs)
    {
        //friendsOnline = friendsManager.GetOnlineFriendsByIDs(friendOnlineIDs);

        //OnlineChanged(friendsOnline);

        //for (int i = 0; i < friendsOnline.Count; i++)
        //{
        //    var id = friendsOnline[i].id;

        //    IdToNicknameMap[id] = friendsOnline[i].nick;
        //}
    }
}
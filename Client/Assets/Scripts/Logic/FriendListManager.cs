using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    Online, Offline
}

public struct FriendProfileData
{
    public UserData userData;
    public Status status;
}

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/FriendListManager")]
public class FriendListManager : Logic
{
    private NetworkManager networkManager;

    private ProfileManager profileManager;

    [NonSerialized]
    private List<UserData> friendList = new List<UserData>();

    private Dictionary<int, UserData> friendsIdMap = new Dictionary<int, UserData>();

    private Queue<UserData> friendRequests = new Queue<UserData>();

    public event Action<FriendProfileData[]> OnFriendsRecived = delegate { };
    public event Action<UserData> OnFriendRequest = delegate { };
    public event Action<UserData> OnAddToFriends = delegate { };

    public override void Init()
    {
        networkManager = LogicManager.GetLogicComponent<NetworkManager>();
        profileManager = LogicManager.GetLogicComponent<ProfileManager>();

        friendList = new List<UserData>();
    }

    public override void MyOnEnable()
    {
        profileManager.OnReciveOnlneFriends += ProfileManager_OnReciveOnlneFriends;
        User.OnAddToFriends += User_OnAddToFriendsClick;

        networkManager.On(ServerEvents.ADD_TO_FRIENDS_REQUEST, AddToFriendsRequest);
    }

    public override void MyOnDisable()
    {
        profileManager.OnReciveOnlneFriends -= ProfileManager_OnReciveOnlneFriends;
        User.OnAddToFriends -= User_OnAddToFriendsClick;

        networkManager.Off(ServerEvents.ADD_TO_FRIENDS_REQUEST, AddToFriendsRequest);
    }

    private void ProfileManager_OnReciveOnlneFriends(int[] onlineFriendIds)
    {
        friendList = FriendsTableAccessor.GetFriendsList();
        Debug.Log("FRIENDS COUNT: " + friendList.Count);

        var friendsProfileDatas = new FriendProfileData[friendList.Count];

        for (int i = 0; i < friendList.Count; i++)
        {
            var item = friendList[i];
            friendsIdMap.Add(item.id, item);

            var status = Status.Offline;

            for (int j = 0; j < onlineFriendIds.Length; j++)
            {
                status = friendsIdMap.ContainsKey(onlineFriendIds[i]) ? Status.Online : Status.Offline;
            }

            friendsProfileDatas[i] = new FriendProfileData { userData = item, status = status };
        }

        OnFriendsRecived(friendsProfileDatas);
    }

    private void User_OnAddToFriendsClick(UserData userData)
    {
        var json = JsonConvert.SerializeObject(new AddToFriendsRequest { id = userData.id, request = true });
        networkManager.Send(ClientEvents.ADD_TO_FRIENDS_REQUEST, json);
    }

    private void AddToFriendsRequest(NetworkMessage networkMessage)
    {
        var response = JsonConvert.DeserializeObject<AddToFriendsResponse>(networkMessage.jsonMessage);

        var userData = new UserData
        {
            id = response.id,
            nick = response.nick
        };

        if (response.response)
        {
            Debug.Log("FREND REQUEST FROM " + userData.nick);

            friendRequests.Enqueue(userData);

            OnFriendRequest(userData);
        }
        else
        {
            friendList.Add(userData);
            Debug.Log("Added to friends list: " + userData.nick);
        }
    }

    public List<UserData> GetOnlineFriendsByIDs(int[] ids)
    {
        var friendsOnline = new List<UserData>(ids.Length);

        foreach (var item in ids)
        {
            var userData = friendsIdMap[item];
            friendsOnline.Add(userData);
        }

        return friendsOnline;
    }

    public void AcceptFriend(UserData userData)
    {
        var json = JsonConvert.SerializeObject(new AddToFriendsRequest { id = userData.id, request = false });
        networkManager.Send(ClientEvents.ADD_TO_FRIENDS_REQUEST, json);

        friendList.Add(userData);

        friendsIdMap.Add(userData.id, userData);

        OnAddToFriends(userData);

        friendRequests.Dequeue();

        FriendsTableAccessor.AddFriend(userData);

        if (friendRequests.Count > 0)
        {
            OnFriendRequest(friendRequests.Dequeue());
        }
    }

    public void DeclineFriend()
    {
        friendRequests.Dequeue();

        if (friendRequests.Count > 0)
        {
            OnFriendRequest(friendRequests.Dequeue());
        }
    }
}
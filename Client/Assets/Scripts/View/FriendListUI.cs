using MEC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FriendListUI : MonoBehaviour
{
    [SerializeField]
    private ProfileManager profile;

    [SerializeField]
    private User userUIPrefab;

    [SerializeField]
    private Text onlineCountText;

    [SerializeField]
    private Transform parent;

    private List<User> friendsUI = new List<User>();

    private List<UserData> onlineUserDatas;

    private List<FriendProfileData> friendsProfileDatas;

    private void OnEnable()
    {
        FriendListManager.OnFriendsRecived += FriendListManager_OnFriendsRecived;
        OnlineManager.OnlineChanged += OnlineManager_OnlineChanged;
    }

    private void OnDisable()
    {
        FriendListManager.OnFriendsRecived -= FriendListManager_OnFriendsRecived;
        OnlineManager.OnlineChanged -= OnlineManager_OnlineChanged;
    }

    private void FriendListManager_OnFriendsRecived(FriendProfileData[] friendsProfileDatas)
    {
        onlineCountText.text = friendsProfileDatas.Length.ToString();

        for (int i = 0; i < friendsUI.Count; i++)
        {
            Destroy(friendsUI[i].gameObject);
        }

        friendsUI.Clear();

        for (int i = 0; i < friendsProfileDatas.Length; i++)
        {
            var user = Instantiate(userUIPrefab, parent);
            user.Init(friendsProfileDatas[i]);
            Debug.Log("Online: " + friendsProfileDatas[i].userData.nick);
            friendsUI.Add(user);
        }

        this.friendsProfileDatas = friendsProfileDatas.ToList();
    }

    private void OnlineManager_OnlineChanged(List<UserData> userDatas)
    {
        onlineCountText.text = onlineUserDatas.Count.ToString();

        for (int i = 0; i < friendsUI.Count; i++)
        {
            Destroy(friendsUI[i].gameObject);
        }

        friendsUI.Clear();

        for (int i = 0; i < onlineUserDatas.Count; i++)
        {
            var user = Instantiate(userUIPrefab, parent);
            user.Init(onlineUserDatas[i]);
            Debug.Log("Online: " + onlineUserDatas[i].nick);
            friendsUI.Add(user);
        }
    }
}
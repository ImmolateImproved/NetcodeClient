using System;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    [SerializeField]
    private Text nicknameText;

    public UserData userData;

    public static event Action<UserData> OnClick = delegate { };
    public static event Action<UserData> OnAddToFriends = delegate { };

    public void Init(UserData userData)
    {
        this.userData = userData;
        nicknameText.text = userData.nick;
    }

    public void Init(FriendProfileData profileData)
    {
        this.userData = profileData.userData;
        nicknameText.text = $"{userData.nick}. Status: {profileData.status}";
    }

    public void OnClickRiseEvent()
    {
        OnClick(userData);
    }

    public void OnAddToFriendsRiseEvent()
    {
        OnAddToFriends(userData);
    }
}
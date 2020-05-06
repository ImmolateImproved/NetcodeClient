using System;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    [SerializeField]
    private Text nicknameText;

    public UserData userData;

    public static event Action<UserData> OnClick = delegate { };

    public void Init(UserData userData)
    {
        this.userData = userData;
        nicknameText.text = $"{userData.id} {userData.nick}";
    }

    public void OnClickRiseEvent()
    {
        OnClick(userData);
    }
}
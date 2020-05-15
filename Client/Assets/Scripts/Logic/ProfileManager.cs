using MEC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    private Socket socket;

    [SerializeField]
    private CommandManager commandManager;

    private ProfileDataFromServer myProfileData;

    public static event Action<UserData> OnReciveProfileData = delegate { };

    public static event Action<int[]> OnReciveOnlneFriends = delegate { };

    private void OnEnable()
    {
        socket.On(ServerEvents.NICK_RESPONSE, NickResponseHandler);
        socket.On(ServerEvents.PROFILE_DATA_RESPONSE, ProfileDataResponseHandler);

        commandManager.On("/nick", SendNickname);
    }

    private void OnDisable()
    {
        socket.Off(ServerEvents.NICK_RESPONSE, NickResponseHandler);
        socket.Off(ServerEvents.PROFILE_DATA_RESPONSE, ProfileDataResponseHandler);

        commandManager.Off("/nick", SendNickname);
    }

    private void ProfileDataResponseHandler(NetworkMessage networkMessage)
    {
        myProfileData = JsonConvert.DeserializeObject<ProfileDataFromServer>(networkMessage.jsonMessage);

        var profileData = new UserData
        {
            id = myProfileData.id,
            nick = myProfileData.nick
        };

        OnReciveProfileData(profileData);

        OnReciveOnlneFriends(myProfileData.friendOnlineIDs);
    }

    private void NickResponseHandler(NetworkMessage networkMessage)
    {
        var result = JsonConvert.DeserializeObject<NickResponse>(networkMessage.jsonMessage).result;

        if (!result)
        {
            Debug.Log("НИК ЗАНЯТ");
        }
    }

    private void SendNickname(string nick)
    {
        myProfileData.nick = nick;
        var nickMessage = new NicknameData { nickname = nick };
        var nickJson = JsonConvert.SerializeObject(nickMessage);

        Debug.Log("Send NICK   " + nickJson);

        socket.Send(ClientEvents.SEND_NICKNAME, nickJson);
    }

    public void SendNickName()
    {
        Timing.CallDelayed(0.3f, () => SendNickname(myProfileData.nick));
    }

    public void SetNickname(string nick)
    {
        Debug.Log("SET NICK: " + nick);
        myProfileData.nick = nick;
    }
}

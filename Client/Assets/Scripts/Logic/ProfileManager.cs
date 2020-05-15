using MEC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/ProfileManager")]
public class ProfileManager : Logic
{
    private Socket socket;

    private CommandManager commandManager;

    private ProfileDataFromServer myProfileData;

    public event Action<UserData> OnReciveProfileData = delegate { };

    public event Action<int[]> OnReciveOnlneFriends = delegate { };

    public override void Init()
    {
        socket = LogicManager.GetLogicComponent<Socket>();
        commandManager = LogicManager.GetLogicComponent<CommandManager>();
    }

    public override void MyOnEnable()
    {
        socket.On(ServerEvents.NICK_RESPONSE, NickResponseHandler);
        socket.On(ServerEvents.PROFILE_DATA_RESPONSE, ProfileDataResponseHandler);

        commandManager.On("/nick", SendNickname);
    }

    public override void MyOnDisable()
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
}

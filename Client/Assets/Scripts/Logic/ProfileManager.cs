using MEC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/ProfileManager")]
public class ProfileManager : Logic
{
    private NetworkManager networkManager;

    private CommandManager commandManager;

    private ProfileDataFromServer myProfileData;

    public event Action<UserData> OnReciveProfileData = delegate { };

    public event Action<int[]> OnReciveOnlneFriends = delegate { };

    public override void Init()
    {
        networkManager = LogicManager.GetLogicComponent<NetworkManager>();
        commandManager = LogicManager.GetLogicComponent<CommandManager>();
    }

    public override void MyOnEnable()
    {
        networkManager.On(ServerEvents.NICK_RESPONSE, NickResponseHandler);
        networkManager.On(ServerEvents.PROFILE_DATA_RESPONSE, ProfileDataResponseHandler);

        commandManager.On("/nick", SendNickname);
    }

    public override void MyOnDisable()
    {
        networkManager.Off(ServerEvents.NICK_RESPONSE, NickResponseHandler);
        networkManager.Off(ServerEvents.PROFILE_DATA_RESPONSE, ProfileDataResponseHandler);

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

        networkManager.Send(ClientEvents.SEND_NICKNAME, nickJson);
    }
}

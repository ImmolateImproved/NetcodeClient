using Newtonsoft.Json;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/UserSearcher")]
public class UserSearcher : Logic
{
    private Socket socket;

    private CommandManager commandManager;

    public event Action<UserData> OnUserFinded = delegate { };

    public override void MyOnEnable()
    {
        socket = LogicManager.GetLogicComponent<Socket>();
        commandManager = LogicManager.GetLogicComponent<CommandManager>();

        socket.On(ServerEvents.SEARCH_USER_BY_NICK_RESPONSE, SearchUserResponse);

        commandManager.On("/search", OnUserSearch);
    }

    public override void MyOnDisable()
    {
        socket.Off(ServerEvents.SEARCH_USER_BY_NICK_RESPONSE, SearchUserResponse);

        commandManager.Off("/search", OnUserSearch);
    }

    private void OnUserSearch(string nick)
    {
        var json = JsonConvert.SerializeObject(new UserNick { nick = nick });

        socket.Send(ClientEvents.SEARCH_USER_BY_NICK_REQUEST, json);
    }

    private void SearchUserResponse(NetworkMessage networkMessage)
    {
        var userData = JsonConvert.DeserializeObject<UserData>(networkMessage.jsonMessage);

        OnUserFinded(userData);
    }
}
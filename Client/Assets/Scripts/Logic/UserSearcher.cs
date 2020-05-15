using Newtonsoft.Json;
using System;
using UnityEngine;

public class UserSearcher : MonoBehaviour
{
    [SerializeField]
    private Socket socket;

    [SerializeField]
    CommandManager commandManager;

    public static event Action<UserData> OnUserFinded = delegate { };

    private void OnEnable()
    {
        socket.On(ServerEvents.SEARCH_USER_BY_NICK_RESPONSE, SearchUserResponse);

        commandManager.On("/search", OnUserSearch);
    }

    private void OnDisable()
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
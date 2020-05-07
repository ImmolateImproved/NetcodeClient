using Newtonsoft.Json;
using UnityEngine;
using System;

public class Authentication : MonoBehaviour
{
    [SerializeField]
    private Socket socket;

    [SerializeField]
    private CommandManager commandManager;

    private UserData userData;

    public static event Action<UserData> OnAuthentification = delegate { };

    private void OnEnable()
    {
        socket.On(ServerEvents.ID, OnReciveID);
        commandManager.On("/nick", SendNickname);
    }

    private void OnDisable()
    {
        socket.Off(ServerEvents.ID, OnReciveID);
        commandManager.Off("/nick", SendNickname);
    }

    private void OnReciveID(NetworkMessage networkMessage)
    {
        var message = JsonConvert.DeserializeObject<ReciveID>(networkMessage.jsonMessage);
        userData.id = message.id;
        SendNickname(userData.nick);
    }

    private void SendNickname(string nick)
    {
        userData.nick = nick;
        OnAuthentification(userData);
        var nickMessage = new AuthData { nickname = nick };
        var nickJson = JsonConvert.SerializeObject(nickMessage);

        Debug.Log("NICK   " + nickJson);

        socket.Send(ClientEvents.SEND_NICKNAME, nickJson);
    }

    public void SetNickname(string nick)
    {
        Debug.Log(nick);
        userData.nick = nick;
    }
}
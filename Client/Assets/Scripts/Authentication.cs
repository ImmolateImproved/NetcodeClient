using Newtonsoft.Json;
using UnityEngine;
using System;
using MEC;

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
        Debug.Log("RECIVED ID:" + userData.id);
        OnAuthentification(userData);
    }

    private void SendNickname(string nick)
    {
        userData.nick = nick;
        var nickMessage = new AuthData { nickname = nick };
        var nickJson = JsonConvert.SerializeObject(nickMessage);

        Debug.Log("Send NICK   " + nickJson);

        socket.Send(ClientEvents.SEND_NICKNAME, nickJson);
    }

    public void SendNickName()
    {
        Timing.CallDelayed(0.3f, () => SendNickname(userData.nick));
    }

    public void SetNickname(string nick)
    {
        Debug.Log("SET NICK: " + nick);
        userData.nick = nick;
    }
}
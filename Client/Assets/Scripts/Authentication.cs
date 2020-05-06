using Newtonsoft.Json;
using UnityEngine;
using System;

public class Authentication
{
    private Socket socket;

    private CommandManager commandManager;

    public static event Action<string> OnAuthentification = delegate { };

    public Authentication(Socket socket, CommandManager commandManager)
    {
        this.socket = socket;
        this.commandManager = commandManager;

        this.commandManager.On("/nick", ReciveNickname);
    }

    public void OnDisable()
    {
        commandManager.Off("/nick", ReciveNickname);
    }

    private void ReciveNickname(string nick)
    {
        OnAuthentification(nick);
        var nickMessage = new AuthData { nickname = nick };
        var nickJson = JsonConvert.SerializeObject(nickMessage);

        Debug.Log("NICK   " + nickJson);

        socket.Send(ClientEvents.SEND_NICKNAME, nickJson);
    }
}
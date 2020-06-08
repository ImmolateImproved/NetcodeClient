using Newtonsoft.Json;
using System;
using UnityEngine;

public abstract class ChatBase : IChat
{
    protected NetworkManager networkManager;
    protected OnlineManager onlineManager;

    public event Action<string, string> OnReciveMessage = delegate { };

    public virtual void Init()
    {
        networkManager = LogicManager.GetLogicComponent<NetworkManager>();
        onlineManager = LogicManager.GetLogicComponent<OnlineManager>();

        networkManager.On(ServerEvents.CHAT_MSG, ReciveMessage);
    }

    public virtual void Dispose()
    {
        networkManager.Off(ServerEvents.CHAT_MSG, ReciveMessage);
    }

    public void ReciveMessage(NetworkMessage networkMessage)
    {
        var message = JsonConvert.DeserializeObject<ChatMessageIn>(networkMessage.jsonMessage);

        var nick = onlineManager.GetNickById(message.id);

        OnReciveMessage(nick, message.message);
    }

    public void Send(string message)
    {
        if (message[0] == '/')
            return;

        var netEventData = ProduceSendData(message);
        networkManager.Send(netEventData.type, netEventData.networkMessage.jsonMessage);
    }

    protected abstract NetworkEventData ProduceSendData(string message);
}
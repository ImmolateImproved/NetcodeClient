using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

public class PrivateChat : ChatBase, IPrivateChat
{
    private Dictionary<int, StringBuilder> privateMessages = new Dictionary<int, StringBuilder>();

    public UserData? MessageReceiver { get; private set; }

    protected override NetworkEventData ProduceSendData(string message)
    {
        Debug.Log("To ID: " + MessageReceiver.Value.id);

        var chatPrivateMessage = new PrivateMessage { toId = MessageReceiver.Value.id, message = message };
        var json = JsonConvert.SerializeObject(chatPrivateMessage);

        var netEventData = new NetworkEventData
        {
            type = ClientEvents.PRIVATE_MSG,
            networkMessage = new NetworkMessage
            {
                jsonMessage = json
            }
        };

        return netEventData;
    }

    public void SetMesageReceiver(UserData userData)
    {
        MessageReceiver = userData;
    }

    public void SavePrivateMessages(string message)
    {
        if (MessageReceiver == null)
            return;

        var id = MessageReceiver.Value.id;

        if (privateMessages.TryGetValue(id, out var builder))
        {
            builder.Append(message);
        }
        else
        {
            var strBuilder = new StringBuilder(50);
            privateMessages[id] = strBuilder.Append(message);
        }
    }

    public string GetMessages()
    {
        var id = MessageReceiver.Value.id;

        if (privateMessages.TryGetValue(id, out var builder))
        {
            return builder.ToString();
        }
        else
        {
            return "";
        }
    }
}
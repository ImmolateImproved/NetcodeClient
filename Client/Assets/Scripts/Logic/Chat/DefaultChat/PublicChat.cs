using UnityEngine;
using Newtonsoft.Json;
using System;

public class PublicChat : ChatBase, IPublicChat
{
    protected override NetworkEventData ProduceSendData(string message)
    {
        var chatMessage = new PublicChatMessageOut { message = message };
        var json = JsonConvert.SerializeObject(chatMessage);

        var netEventData = new NetworkEventData
        {
            type = ClientEvents.CHAT_MSG,
            networkMessage = new NetworkMessage
            {
                jsonMessage = json
            }
        };

        return netEventData;
    }
}
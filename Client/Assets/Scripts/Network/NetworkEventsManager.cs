using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public struct NetworkMessage
{
    public string jsonMessage;
}

public class NetworkEventsManager
{
    Dictionary<int, Action<NetworkMessage>> networkEvents = new Dictionary<int, Action<NetworkMessage>>();

    public void ReciveMessage(int type, NetworkMessage message)
    {
        if (networkEvents.TryGetValue(type, out var action))
        {
            action.Invoke(message);
        }
    }

    public void On(int type, Action<NetworkMessage> action)
    {
        if (networkEvents.TryGetValue(type, out var act))
        {
            act += action;
        }
        else
        {
            networkEvents.Add(type, action);
        }
    }

    public void Off(int type, Action<NetworkMessage> action)
    {
        if (networkEvents.TryGetValue(type, out var act))
        {
            act -= action;
        }
    }
}
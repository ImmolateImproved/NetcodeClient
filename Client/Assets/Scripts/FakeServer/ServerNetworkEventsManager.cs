using System;
using System.Collections.Generic;

public class ServerNetworkEventsManager
{
    private Dictionary<int, Action<NetworkMessage>> networkEvents = new Dictionary<int, Action<NetworkMessage>>();

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
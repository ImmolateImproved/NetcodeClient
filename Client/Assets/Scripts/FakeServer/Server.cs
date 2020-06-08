using System;
using System.Collections.Generic;

public class Server : Logic
{
    private int idCounter;

    private NetworkEventsManager eventsManager;

    private Dictionary<ITCPSocket, Client> socketToClientMap = new Dictionary<ITCPSocket, Client>();

    public override void Init()
    {
        eventsManager = new NetworkEventsManager();
    }

    public void Connect(ITCPSocket socket)
    {
        var client = new Client(socket, idCounter);
        socketToClientMap.Add(socket, client);
        idCounter++;
    }

    public void On(int type, Action<NetworkMessage> action)
    {
        eventsManager.On(type, action);
    }

    public void Off(int type, Action<NetworkMessage> action)
    {
        eventsManager.Off(type, action);
    }
}
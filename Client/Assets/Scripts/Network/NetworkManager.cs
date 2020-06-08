using UnityEngine;
using System.Collections.Generic;
using System;
using MEC;

public enum UrlType
{
    WS, Auth, Reg
}

public struct NetworkEventData
{
    public int type;
    public NetworkMessage networkMessage;
}

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/NetworkManager")]
public class NetworkManager : Logic
{
    [SerializeField]
    private UrlData localURL;
    [SerializeField]
    private UrlData remoteURL;

    [SerializeField]
    private bool isLocal;
    
    private ITCPSocket connection;

    private object queueLock = new object();

    private Queue<NetworkEventData> networkEvents = new Queue<NetworkEventData>();

    private NetworkEventsManager eventsManager;

    private CoroutineHandle reciveMessageHandle;

    public bool IsOpen
    {
        get
        {
            return connection != null && connection.IsOpen;
        }
    }

    public bool IsLocal => isLocal;

    public override void Init()
    {
        eventsManager = new NetworkEventsManager();
    }

    public override void MyOnEnable()
    {
        reciveMessageHandle = Timing.RunCoroutine(ReciveNetworkMessages());
    }

    public override void MyOnDisable()
    {
        Timing.KillCoroutines(reciveMessageHandle);
        connection?.Close();
    }

    public override void MyOnDestroy()
    {
        connection = null;
    }

    private void ReciveMessage(string message)
    {
        var msg = message.Split('\n');
        for (int i = 0; i < msg.Length; i++)
        {
            var typeStr = msg[i].Substring(0, 4);
            var json = msg[i].Substring(4);

            Debug.Log($"RECIVED {typeStr}");

            if (int.TryParse(typeStr, out var type))
            {
                Debug.Log($"RECIVED {typeStr} {json} ");

                var netwokrEvent = new NetworkEventData { type = type, networkMessage = new NetworkMessage { jsonMessage = json } };
                lock (queueLock)
                {
                    networkEvents.Enqueue(netwokrEvent);
                }
            }
        }
    }

    private IEnumerator<float> ReciveNetworkMessages()
    {
        while (true)
        {
            lock (queueLock)
            {
                if (networkEvents.Count > 0)
                {
                    var networkEvent = networkEvents.Dequeue();
                    var networkMessage = new NetworkMessage { jsonMessage = networkEvent.networkMessage.jsonMessage };
                    eventsManager.ReciveMessage(networkEvent.type, networkMessage);
                }
            }

            yield return Timing.WaitForOneFrame;
        }
    }

    public static string GetUrl(UrlType urlType, string url)
    {
        switch (urlType)
        {
            case UrlType.WS:
                {
                    return $"ws://{url}/ws";
                }

            case UrlType.Auth:
                {
                    return $"http://{url}/authorization";
                }

            case UrlType.Reg:
                {
                    return $"http://{url}/registration";
                }
        }

        return null;
    }

    public void Connect()
    {
        var url = isLocal ? localURL : remoteURL;

        connection = new TCPSocket(url);
        connection.OnMessageReceive += ReciveMessage;

        connection.Connect();
    }

    public void Connect(string tokenJson)
    {
        Connect();

        Send(ClientEvents.TOKEN_AUTH, tokenJson);
    }

    public void On(int type, Action<NetworkMessage> action)
    {
        eventsManager.On(type, action);
    }

    public void Off(int type, Action<NetworkMessage> action)
    {
        eventsManager.Off(type, action);
    }

    public void Send(int type, string jsonMessage)
    {
        var msg = type + jsonMessage;
        connection?.Send(msg);
    }
}
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System;
using MEC;

public enum UrlType
{
    WS, Auth, Reg
}

public struct NetworkEvents
{
    public int type;
    public NetworkMessage networkMessage;
}

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/Socket")]
public class Socket : Logic
{
    [SerializeField]
    private string localWSUrl = "192.168.0.105:8081";

    [SerializeField]
    private string remoteWSUrl = "46.119.183.31:55443";

    [SerializeField]
    private bool isLocal;

    private WebSocket connection;

    private object queueLock = new object();

    private Queue<NetworkEvents> networkEvents = new Queue<NetworkEvents>();

    private NetworkEventsManager eventsManager;

    private CoroutineHandle reciveMessageHandle;

    public bool IsOpen
    {
        get
        {
            return connection != null && connection.IsAlive;
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

    private void ReciveMessage(object sender, MessageEventArgs e)
    {
        var bytes = e.RawData;

        var str = Encoding.UTF8.GetString(bytes);
        var messages = str.Split('\n');
        for (int i = 0; i < messages.Length; i++)
        {
            var typeStr = messages[i].Substring(0, 4);
            var json = messages[i].Substring(4);

            Debug.Log($"RECIVED {typeStr}");

            if (int.TryParse(typeStr, out var type))
            {
                Debug.Log($"RECIVED {typeStr} {json} ");

                var netwokrEvent = new NetworkEvents { type = type, networkMessage = new NetworkMessage { jsonMessage = json } };
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

    public string GetUrl(UrlType urlType, string url)
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
        var url = isLocal ? localWSUrl : remoteWSUrl;
        url = GetUrl(UrlType.WS, url);

        connection = new WebSocket(url);
        connection.OnMessage += ReciveMessage;

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
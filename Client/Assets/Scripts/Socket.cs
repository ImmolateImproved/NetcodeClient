using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System;

public class Socket : MonoBehaviour
{
    private string Url;

    [SerializeField]
    private string localUrl = "ws://192.168.0.105:8081/ws";

    [SerializeField]
    private string remoteUrl = "ws://46.119.183.31:55443/ws";

    [SerializeField]
    private bool isLocal;

    private WebSocket connection;

    private NetworkEventsManager eventsManager;

    private void Awake()
    {
        Url = isLocal ? localUrl : remoteUrl;

        eventsManager = new NetworkEventsManager();
    }

    private void OnDisable()
    {
        connection?.Close();
    }

    public void Connect()
    {
        connection = new WebSocket(Url);
        connection.Connect();
        connection.OnMessage += ReciveMessage;
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

    private void ReciveMessage(object sender, MessageEventArgs e)
    {
        var bytes = e.RawData;

        var typeBytes = bytes.SubArray(0, 4);
        var typeStr = Encoding.UTF8.GetString(typeBytes);

        var jsonBytes = bytes.SubArray(4, bytes.Length - 4);
        var json = Encoding.UTF8.GetString(jsonBytes);

        Debug.Log($"RECIVED {typeStr}");

        if (int.TryParse(typeStr, out var type))
        {
            Debug.Log($"RECIVED {typeStr} {json} ");

            var networkMessage = new NetworkMessage { jsonMessage = json };
            eventsManager.ReciveMessage(type, networkMessage);
        }
    }
}
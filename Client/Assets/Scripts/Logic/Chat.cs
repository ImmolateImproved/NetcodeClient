using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/Chat")]
public class Chat : Logic
{
    private Socket socket;

    private OnlineManager onlineManager;

    private UserData? privateMessageUser;

    public UserData? PrivateMessageUser { get => privateMessageUser; }

    public bool IsPublicMode { get; private set; } = true;

    public Dictionary<int, string> privateMessages { get; private set; } = new Dictionary<int, string>();

    public event Action<string> OnSendMessage = delegate { };
    public event Action<string> OnMessageRecive = delegate { };
    public event Action<string> OnPrivateMessageRecive = delegate { };

    private Action<string> ChatAction;

    public override void Init()
    {
        socket = LogicManager.GetLogicComponent<Socket>();
        onlineManager = LogicManager.GetLogicComponent<OnlineManager>();

        ChatAction = PublicChatAction;
    }

    public override void MyOnEnable()
    {
        socket.On(ServerEvents.CHAT_MSG, OnReciveMessage);
        socket.On(ServerEvents.PRIVATE_MSG, OnRecivePrivateMessage);
    }

    public override void MyOnDisable()
    {
        socket.Off(ServerEvents.CHAT_MSG, OnReciveMessage);
        socket.Off(ServerEvents.PRIVATE_MSG, OnRecivePrivateMessage);
    }

    private void OnReciveMessage(NetworkMessage networkMessage)
    {
        var message = JsonConvert.DeserializeObject<ChatMessageIn>(networkMessage.jsonMessage);

        var nick = onlineManager.IdToNicknameMap[message.id];

        OnMessageRecive($"{nick}: {message.message}");
    }

    private void OnRecivePrivateMessage(NetworkMessage networkMessage)
    {
        var message = JsonConvert.DeserializeObject<ChatMessageIn>(networkMessage.jsonMessage);

        var nick = onlineManager.IdToNicknameMap[message.id];

        SavePrivateMessages(message.id, message.message, nick);

        OnPrivateMessageRecive($"{nick}: {message.message}");
    }

    private void SavePrivateMessages(int id, string message, string targetName)
    {
        if (privateMessages.TryGetValue(id, out var text))
        {
            if (targetName == null)
            {
                privateMessages[id] += $"{text} Вы: {message} \n";
            }
            else
            {
                privateMessages[id] += $"{text}{targetName} : {message} \n";
            }
        }
    }

    private void PublicChatAction(string message)
    {
        var chatMessage = new PublicChatMessageOut { message = message };
        var json = JsonConvert.SerializeObject(chatMessage);

        socket.Send(ClientEvents.CHAT_MSG, json);
    }

    private void PrivateChatAction(string message)
    {
        if (privateMessageUser != null)
        {
            Debug.Log("To ID: " + privateMessageUser.Value.id);
            SavePrivateMessages(privateMessageUser.Value.id, message, null);

            var chatPrivateMessage = new PrivateMessage { toId = privateMessageUser.Value.id, message = message };
            var json = JsonConvert.SerializeObject(chatPrivateMessage);

            socket.Send(ClientEvents.PRIVATE_MSG, json);
        }
    }

    private void UpdateChatAction()
    {
        if (IsPublicMode)
        {
            ChatAction = PublicChatAction;
        }
        else
        {
            ChatAction = PrivateChatAction;
        }
    }

    public void ChangeChatMode(bool isPublic)
    {
        if (PrivateMessageUser == null)
            return;

        Debug.Log("CAHGED CHAT MODE");
        IsPublicMode = isPublic;
        UpdateChatAction();
    }

    public void SetPrivateMessageUser(UserData userData)
    {
        privateMessageUser = userData;
        IsPublicMode = false;

        if (!privateMessages.TryGetValue(userData.id, out var text))
        {
            privateMessages[userData.id] = "";
        }

        UpdateChatAction();
    }

    public void Send(string message)
    {
        if (message.Length == 0)
            return;

        OnSendMessage(message);

        if (message[0] == '/')
            return;

        ChatAction(message);
    }
}
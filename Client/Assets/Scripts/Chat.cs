using UnityEngine;
using Newtonsoft.Json;
using System;

public class Chat : MonoBehaviour
{
    [SerializeField]
    private Socket socket;

    [SerializeField]
    private OnlineManager onlineManager;

    private UserData myUserData = new UserData { id = -1, nick = "USERNAME" };

    private UserData? privateMessageUser;

    public UserData? PrivateMessageUser { get => privateMessageUser; }

    public bool IsPubliMode { get; private set; } = true;

    public string Nickname
    {
        get { return myUserData.nick; }
    }

    public static event Action<string> OnSendMessage = delegate { };
    public static event Action<string> OnMessageRecive = delegate { };
    public static event Action OnChatModeChanged = delegate { };

    private Action<string> ChatAction;

    private void Awake()
    {
        ChatAction = PublicChatAction;
    }

    private void OnEnable()
    {
        Authentication.OnAuthentification += SetUserdata;
        User.OnClick += SetPrivateMessageUser;

        socket.On(ServerEvents.CHAT_MSG, OnReciveMessage);
    }

    private void OnDisable()
    {
        Authentication.OnAuthentification -= SetUserdata;
        User.OnClick -= SetPrivateMessageUser;

        socket.Off(ServerEvents.CHAT_MSG, OnReciveMessage);
    }

    private void SetUserdata(UserData userData)
    {
        myUserData = userData;
    }

    private void OnReciveMessage(NetworkMessage networkMessage)
    {
        var message = JsonConvert.DeserializeObject<ChatMessageIn>(networkMessage.jsonMessage);

        var nick = onlineManager.IdToNicknameMap[message.id];

        OnMessageRecive($"{nick}: {message.message}");
    }

    private void SetPrivateMessageUser(UserData userData)
    {
        privateMessageUser = userData;
        IsPubliMode = false;
        UpdateChatAction();
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
            var chatPrivateMessage = new PrivateMessage { toId = privateMessageUser.Value.id, message = message };
            var json = JsonConvert.SerializeObject(chatPrivateMessage);

            socket.Send(ClientEvents.PRIVATE_MSG, json);
        }
    }

    private void UpdateChatAction()
    {
        OnChatModeChanged();
        if (IsPubliMode)
        {
            ChatAction = PublicChatAction;
        }
        else
        {
            ChatAction = PrivateChatAction;
        }
    }

    public void ChangeChatMode()
    {
        if (PrivateMessageUser == null)
            return;

        IsPubliMode = !IsPubliMode;
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
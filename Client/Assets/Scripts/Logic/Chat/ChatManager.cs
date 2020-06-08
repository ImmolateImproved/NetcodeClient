using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/Chat")]
public class ChatManager : Logic
{
    private IChat activeChat;

    private IPublicChat iPublicChat;
    private IPrivateChat iPrivateChat;

    [SerializeField]
    private ChatFactoryBase chatFactory;

    public bool IsPublicMode { get; private set; }

    public event Action<string> OnSendMessage = delegate { };

    public override void Init()
    {
        IsPublicMode = true;
        SetChatMode(IsPublicMode);
    }

    public override void MyOnDisable()
    {
        iPublicChat.Dispose();
        iPrivateChat.Dispose();
    }

    public IPublicChat GetPublicChat()
    {
        if (iPublicChat == null)
        {
            iPublicChat = chatFactory.GetPublicChat();

            iPublicChat.Init();
        }

        return iPublicChat;
    }

    public IPrivateChat GetPrivateChat()
    {
        if (iPrivateChat == null)
        {
            iPrivateChat = chatFactory.GetPrivateChat();
            iPrivateChat.Init();
        }

        return iPrivateChat;
    }

    public void Send(string message)
    {
        OnSendMessage(message);
        activeChat.Send(message);
    }

    public void SetChatMode(bool isPublicMode)
    {
        IsPublicMode = isPublicMode;

        if (isPublicMode)
        {
            activeChat = iPublicChat;
        }
        else
        {
            activeChat = iPrivateChat;
        }
    }
}
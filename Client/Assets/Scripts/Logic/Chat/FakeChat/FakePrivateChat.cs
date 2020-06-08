using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FakePrivateChat : FakeChatBase, IPrivateChat
{
    private Dictionary<int, StringBuilder> privateMessages = new Dictionary<int, StringBuilder>();

    public UserData? MessageReceiver { get; private set; }

    public override void Init()
    {
        base.Init();

        MessageReceiver = new UserData { id = 322, nick = "Solo" };
    }

    public void SetMesageReceiver(UserData userData)
    {
        MessageReceiver = userData;
    }

    public void SavePrivateMessages(string message)
    {
        if (MessageReceiver == null)
            return;

        var id = MessageReceiver.Value.id;

        if (privateMessages.TryGetValue(id, out var builder))
        {
            builder.Append(message);
        }
        else
        {
            var strBuilder = new StringBuilder(50);
            privateMessages[id] = strBuilder.Append(message);
        }
    }

    public string GetMessages()
    {
        var id = MessageReceiver.Value.id;

        if (privateMessages.TryGetValue(id, out var builder))
        {
            return builder.ToString();
        }
        else
        {
            return "";
        }
    }
}
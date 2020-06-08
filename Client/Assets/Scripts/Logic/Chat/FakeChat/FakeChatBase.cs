using System;
using UnityEngine;

public abstract class FakeChatBase : IChat
{
    public event Action<string, string> OnReciveMessage = delegate { };

    public virtual void Init()
    {

    }

    public virtual void Dispose()
    {

    }

    public virtual void MyOnEnable()
    {

    }

    public void ReciveMessage(NetworkMessage networkMessage)
    {
        OnReciveMessage("Unnamed", networkMessage.jsonMessage);
    }

    public void Send(string message)
    {
        ReciveMessage(new NetworkMessage { jsonMessage = message });
    }
}
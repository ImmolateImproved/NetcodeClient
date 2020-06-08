using UnityEngine;

public abstract class ChatFactoryBase : ScriptableObject
{
    public IPublicChat publicChat;
    public IPrivateChat privateChat;

    public abstract IPublicChat GetPublicChat();

    public abstract IPrivateChat GetPrivateChat();
}
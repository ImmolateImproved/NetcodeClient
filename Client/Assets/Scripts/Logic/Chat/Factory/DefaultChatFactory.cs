using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DefaultChatFactory")]
public class DefaultChatFactory : ChatFactoryBase
{
    public override IPublicChat GetPublicChat()
    {
        if (publicChat == null)
        {
            publicChat = new PublicChat();
            publicChat.Init();
            publicChat.Dispose();
        }

        return publicChat;
    }

    public override IPrivateChat GetPrivateChat()
    {
        if (privateChat == null)
        {
            privateChat = new PrivateChat();
            privateChat.Init();
            privateChat.Dispose();
        }

        return privateChat;
    }
}
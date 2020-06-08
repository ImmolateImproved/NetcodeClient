using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FakeChatFactory")]
public class FakeChatFactory : ChatFactoryBase
{
    public override IPublicChat GetPublicChat()
    {
        if (publicChat == null)
        {
            publicChat = new FakePublicChat();
            publicChat.Init();
            publicChat.Dispose();
        }

        return publicChat;
    }

    public override IPrivateChat GetPrivateChat()
    {
        if (privateChat == null)
        {
            privateChat = new FakePrivateChat();
            privateChat.Init();
            privateChat.Dispose();
        }

        return privateChat;
    }
}
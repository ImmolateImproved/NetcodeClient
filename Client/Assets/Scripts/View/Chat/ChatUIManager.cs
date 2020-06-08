using UnityEngine;

public class ChatUIManager : MonoBehaviour
{
    private ChatManager chatManager;
    private IPublicChat publicChat;
    private IPrivateChat privateChat;

    [SerializeField]
    private ChatUI publicChatUI;

    [SerializeField]
    private ChatUI privateChatUI;

    [SerializeField]
    private TweenableMenuElement privateChatTweener;

    private ChatUI activeChatUI;

    private void Awake()
    {
        chatManager = LogicManager.GetLogicComponent<ChatManager>();

        publicChat = chatManager.GetPublicChat();
        privateChat = chatManager.GetPrivateChat();
    }

    private void Start()
    {
        activeChatUI = publicChatUI;
        publicChatUI.Activate();
    }

    private void OnEnable()
    {
        publicChat.OnReciveMessage += Chat_OnMessageRecive;
        privateChat.OnReciveMessage += Chat_OnPrivateMessageRecive;

        User.OnClick += User_OnClick;
    }

    private void OnDisable()
    {
        publicChat.OnReciveMessage -= Chat_OnMessageRecive;
        privateChat.OnReciveMessage -= Chat_OnPrivateMessageRecive;

        User.OnClick -= User_OnClick;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && activeChatUI.IsActive)
        {
            Send();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetChatMode(!chatManager.IsPublicMode);
        }
    }

    private void User_OnClick(UserData userData)
    {
        privateChat.SetMesageReceiver(userData);
        SetChatMode(false);
    }

    private void Chat_OnPrivateMessageRecive(string nick, string message)
    {
        privateChatUI.Append($"{nick}: {message}\n");
    }

    private void Chat_OnMessageRecive(string nick, string message)
    {
        publicChatUI.Append($"{nick}: {message}\n");
    }

    private void Send()
    {
        var message = activeChatUI.Send(chatManager.IsPublicMode);

        if (message == null)
            return;

        chatManager.Send(message);
    }

    public void SetChatMode(bool isPublicMode)
    {
        chatManager.SetChatMode(isPublicMode);

        if (isPublicMode)
        {
            privateChat.SavePrivateMessages(privateChatUI.GetText());

            activeChatUI = publicChatUI;
            publicChatUI.SetMessageReceiver("Общий:");
        }
        else
        {
            privateChatTweener.Show();

            var userData = privateChat.MessageReceiver.Value;

            activeChatUI = privateChatUI;
            privateChatUI.Append(privateChat.GetMessages());
            privateChatUI.SetMessageReceiver($"To {userData.id} {userData.nick}: ");
        }

        activeChatUI.Activate();
    }
}
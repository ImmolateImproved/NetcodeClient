using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

[System.Serializable]
public struct ChatUI
{
    public InputField inputField;
    public Text chatText;
    public Text toText;
    public Scrollbar chatVerticalScroll;
}

public class ChatUIManager : MonoBehaviour
{
    [SerializeField]
    private Chat chat;

    [SerializeField]
    private ChatUI publicChat;

    [SerializeField]
    private ChatUI privateChat;

    [SerializeField]
    private TweenableUIElement privateChatTweener;

    private ChatUI activeChat;

    private string newPublicMessage;

    private string newPrivateMessage;

    private bool publicChatHasChanged;
    private bool privateChatHasChanged;

    private Coroutine chatCoroutine;

    private void Start()
    {
        activeChat = publicChat;
        publicChat.inputField.ActivateInputField();
    }

    private void OnEnable()
    {
        Chat.OnMessageRecive += Chat_OnMessageRecive;
        Chat.OnPrivateMessageRecive += Chat_OnPrivateMessageRecive;
        Chat.OnChatModeChanged += ChangeChatMode;
        User.OnClick += User_OnClick;

        chatCoroutine = StartCoroutine(UpdateChat());
    }

    private void OnDisable()
    {
        Chat.OnMessageRecive -= Chat_OnMessageRecive;
        Chat.OnPrivateMessageRecive -= Chat_OnPrivateMessageRecive;
        Chat.OnChatModeChanged -= ChangeChatMode;
        User.OnClick -= User_OnClick;

        StopCoroutine(chatCoroutine);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && EventSystem.current.currentSelectedGameObject == activeChat.inputField.gameObject)
        {
            Send();
        }
    }

    private IEnumerator UpdateChat()
    {
        while (true)
        {
            if (publicChatHasChanged)
            {
                publicChatHasChanged = false;
                publicChat.chatText.text += newPublicMessage;
            }

            if (privateChatHasChanged)
            {
                privateChatHasChanged = false;
                privateChat.chatText.text += newPrivateMessage;
            }

            yield return null;
        }
    }

    private void User_OnClick(UserData userData)
    {
        if (chat.privateMessages.TryGetValue(userData.id, out var text))
        {
           // privateChat.chatText.text = text;
        }
        else
        {
            //privateChat.chatText.text = "";
        }
    }

    private void Chat_OnPrivateMessageRecive(string message)
    {
        privateChatHasChanged = true;
        newPrivateMessage = message+ "\n";
    }

    private void Chat_OnMessageRecive(string message)
    {
        publicChatHasChanged = true;
        newPublicMessage = message + "\n";
    }

    public void ChangeChatMode()
    {
        if (chat.IsPublicMode)
        {
            privateChatTweener.HidePanel();

            activeChat = publicChat;
            activeChat.toText.text = "Общий:";
        }
        else
        {
            privateChatTweener.ShowPanel();

            activeChat = privateChat;
            var userData = chat.PrivateMessageUser.Value;
            activeChat.toText.text = $"To {userData.id} {userData.nick}: ";
        }
    }

    public void Send()
    {
        var message = activeChat.inputField.text;

        if (message.Length == 0)
            return;

        activeChat.inputField.text = "";
        activeChat.chatVerticalScroll.value = 0;
        activeChat.inputField.ActivateInputField();

        if (chat.IsPublicMode)
        {
            activeChat.chatText.text += $"Общий: {message}  \n";
        }
        else
        {
            activeChat.chatText.text += $"Вы: {chat.PrivateMessageUser.Value.nick}: {message}  \n";
        }

        chat.Send(message);
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ChatUI : MonoBehaviour
{
    [SerializeField]
    private Chat chat;

    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Text chatText;

    [SerializeField]
    private Text toText;

    [SerializeField]
    private Scrollbar chatVerticalScroll;

    private string newMessage;

    private bool chatHasChanged;

    private Coroutine chatCoroutine;

    private void Start()
    {
        inputField.ActivateInputField();
    }

    private void OnEnable()
    {
        User.OnClick += SetPrivateMessageUser;
        Chat.OnMessageRecive += Chat_OnMessageRecive;
        Chat.OnChatModeChanged += ChangeChatMode;

        chatCoroutine = StartCoroutine(UpdateChat());
    }

    private void OnDisable()
    {
        User.OnClick -= SetPrivateMessageUser;
        Chat.OnMessageRecive -= Chat_OnMessageRecive;
        Chat.OnChatModeChanged -= ChangeChatMode;

        StopCoroutine(chatCoroutine);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && EventSystem.current.currentSelectedGameObject == inputField.gameObject)
        {
            Send();
        }
    }

    private IEnumerator UpdateChat()
    {
        while (true)
        {
            if (chatHasChanged)
            {
                chatHasChanged = false;
                chatText.text += newMessage;
            }

            yield return null;
        }
    }

    private void SetPrivateMessageUser(UserData userData)
    {
        toText.text = userData.id + ": ";
    }

    private void Chat_OnMessageRecive(string message)
    {
        chatHasChanged = true;
        newMessage += message + "\n";
    }

    public void ChangeChatMode()
    {
        if (chat.IsPubliMode)
        {
            toText.text = "Общий:";
        }
        else
        {
            var userData = chat.PrivateMessageUser.Value;
            toText.text = $"To {userData.id} {userData.nick}:";
        }
    }

    public void Send()
    {
        var message = inputField.text;

        if (message.Length == 0)
            return;

        inputField.text = "";
        chatVerticalScroll.value = 0;
        inputField.ActivateInputField();

        if (chat.IsPubliMode)
        {
            chatText.text += $"Общий: {message}  \n";
        }
        else
        {
            chatText.text += $"В личку {chat.PrivateMessageUser.Value.nick}: {message}  \n";
        }

        chat.Send(message);
    }
}
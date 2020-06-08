using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Text chatText;

    [SerializeField]
    private Text toText;

    [SerializeField]
    private Scrollbar chatVerticalScroll;

    public bool IsActive
    {
        get => EventSystem.current.currentSelectedGameObject == inputField.gameObject;
    }

    public string Send(bool isPublicMode)
    {
        var message = inputField.text;

        if (message.Length == 0)
            return null;

        inputField.text = "";
        chatVerticalScroll.value = 0;
        inputField.ActivateInputField();

        if (isPublicMode)
        {
            chatText.text += $"Общий: {message}\n";
        }
        else
        {
            chatText.text += $"Вы: {message}\n";
        }

        return message;
    }

    public string GetText()
    {
        return chatText.text;
    }

    public void Activate()
    {
        inputField.ActivateInputField();
    }

    public void Append(string message)
    {
        chatText.text += message;
    }

    public void SetMessageReceiver(string receiver)
    {
        toText.text = receiver;
    }
}

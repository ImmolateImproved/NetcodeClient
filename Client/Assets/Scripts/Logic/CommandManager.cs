using UnityEngine;
using System;
using System.Collections.Generic;

public class CommandManager : MonoBehaviour
{
    public Dictionary<string, Action<string>> commands = new Dictionary<string, Action<string>>();

    private void OnEnable()
    {
        Chat.OnSendMessage += DeconstructMessage;
    }

    private void OnDisable()
    {
        Chat.OnSendMessage -= DeconstructMessage;
    }

    public void On(string type, Action<string> action)
    {
        if (commands.TryGetValue(type, out var act))
        {
            act += action;
        }
        else
        {
            commands.Add(type, action);
        }
    }

    public void Off(string type, Action<string> action)
    {
        if (commands.TryGetValue(type, out var act))
        {
            act -= action;
        }
    }

    public void DeconstructMessage(string message)
    {
        if (message[0] != '/')
            return;

        var subStr = message.Split(' ');

        if (commands.TryGetValue(subStr[0], out var type))
        {
            type.Invoke(subStr[1]);
        }
    }
}
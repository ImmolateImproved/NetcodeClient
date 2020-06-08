using System;

public interface IChat
{
    void Send(string message);
    void ReciveMessage(NetworkMessage networkMessage);

    void Init();
    void Dispose();
}

public interface IPublicChat : IChat
{
    event Action<string, string> OnReciveMessage;
}

public interface IPrivateChat : IChat
{
    UserData? MessageReceiver { get; }

    void SetMesageReceiver(UserData userData);
    void SavePrivateMessages(string messages);
    string GetMessages();

    event Action<string, string> OnReciveMessage;
}
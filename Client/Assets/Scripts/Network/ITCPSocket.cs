
using System;

public interface ITCPSocket
{
    bool IsOpen { get; }
    void Connect();
    void Send(string message);
    void Close();

    event Action<string> OnMessageReceive;
}
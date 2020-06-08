using System;

public class FakeSocket : ITCPSocket
{
    public bool IsOpen => throw new NotImplementedException();

    public event Action<string> OnMessageReceive;

    public void Close()
    {

    }

    public void Connect()
    {

    }

    public void Send(string message)
    {

    }
}
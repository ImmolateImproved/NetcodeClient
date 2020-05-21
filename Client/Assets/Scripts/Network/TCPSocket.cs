using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Policy;

[System.Serializable]
public struct UrlData
{
    public string ip;
    public int port;
}

public class TCPSocket
{
    private readonly string ip = "192.168.0.105";
    private readonly int port = 8080;

    private TcpClient connection;
    private NetworkStream stream;

    public bool IsOpen => connection.Connected;

    public event Action<string> OnMesageRecive = delegate { };

    public TCPSocket(string ip, int port)
    {
        this.ip = ip;
        this.port = port;
    }

    public TCPSocket(UrlData urlData)
    {
        ip = urlData.ip;
        port = urlData.port;
    }

    public async void Connect()
    {
        connection = new TcpClient();

        connection.Connect(ip, port);
        stream = connection.GetStream();

        await Task.Run(() => ReceiveMessage());
    }

    public void Close()
    {
        if (stream != null)
            stream.Close();
        if (connection != null)
            connection.Close();
    }

    public void Send(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    private void ReceiveMessage()
    {
        while (true)
        {
            var data = new byte[64];
            StringBuilder builder = new StringBuilder();
            do
            {
                var bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            var message = builder.ToString();
            OnMesageRecive(message);
        }
    }
}
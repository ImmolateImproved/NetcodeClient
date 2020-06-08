
public class Client
{
    public ITCPSocket Socket { get; private set; }
    public int Id { get; private set; }
    public string Nickname { get; private set; }

    public Client(ITCPSocket socket, int id)
    {
        Socket = socket;
        Id = id;
    }
}
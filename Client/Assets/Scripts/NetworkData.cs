using Newtonsoft.Json;

struct AuthData
{
    [JsonProperty("n")]
    public string nickname;
}
#region Chat

struct PublicChatMessageOut
{
    [JsonProperty("t")]
    public string message;
}

struct ChatMessageIn
{
    [JsonProperty("f")]
    public int id;

    [JsonProperty("t")]
    public string message;
}

struct PrivateMessage
{
    [JsonProperty("fid")]
    public int toId;

    [JsonProperty("t")]
    public string message;
}
#endregion

struct ReciveID
{
    [JsonProperty("id")]
    public int id;

    [JsonProperty("s")]
    public string status;
}

#region Online Data
public struct UserData
{
    [JsonProperty("id")]
    public int id;

    [JsonProperty("n")]
    public string nick;
}

struct OnlineRequest
{
    [JsonProperty("n")]
    public int hz;
}

struct OnlineResponse
{
    public UserData[] value;
}
#endregion
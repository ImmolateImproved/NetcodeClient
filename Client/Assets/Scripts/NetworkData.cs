using Newtonsoft.Json;

struct AuthData
{
    [JsonProperty("nick")]
    public string nickname;
}
#region Chat

struct PublicChatMessageOut
{
    [JsonProperty("text")]
    public string message;
}

struct ChatMessageIn
{
    [JsonProperty("id")]
    public int id;

    [JsonProperty("text")]
    public string message;
}

struct PrivateMessage
{
    [JsonProperty("id")]
    public int toId;

    [JsonProperty("text")]
    public string message;
}
#endregion

struct ReciveID
{
    [JsonProperty("id")]
    public int id;
}

#region Online Data
[System.Serializable]
public struct UserData
{
    [JsonProperty("id")]
    public int id;

    [JsonProperty("nick")]
    public string nick;
}
#endregion
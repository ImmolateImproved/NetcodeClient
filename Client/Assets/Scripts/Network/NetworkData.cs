using Newtonsoft.Json;

#region Authentication/Registration
public struct AuthenticationStatus
{
    public bool status;
}

[System.Serializable]
struct TokenData
{
    public string token;
}
#endregion

#region Profile
public struct ProfileDataFromServer
{
    public int id;
    public string nick;
    [JsonProperty("friends")]
    public int[] friendOnlineIDs;
}

struct NicknameData
{
    [JsonProperty("nick")]
    public string nickname;
}

struct NickResponse
{
    public bool result;
}

struct AddToFriendsRequest
{
    public int id;
    public bool request;
}

public struct AddToFriendsResponse
{
    public int id;

    public string nick;

    public bool response;
}
#endregion

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

#region Users
public struct UserNick
{
    public string nick;
}

[System.Serializable]
public struct UserData
{
    public int id;

    public string nick;
}
#endregion
public static class ServerEvents
{
    public const int TOKEN_AUTH = 1001;
    public const int NICK_RESPONSE = 1002;
    public const int PROFILE_DATA_RESPONSE = 1003;
    public const int SEARCH_USER_BY_NICK_RESPONSE = 1004;

    public const int ADD_TO_FRIENDS_REQUEST = 1005;
    public const int REMOVE_FRIENDS_REQUEST = 1006;
    public const int FRIENDS_LIST_REQUEST = 1007;

    public const int CHAT_MSG = 2002;
    public const int PRIVATE_MSG = 2003;
    public const int ONLINE_RESPONSE = 2004;
}

public static class ClientEvents
{
    public const int TOKEN_AUTH = 1001;
    public const int SEND_NICKNAME = 1002;
    public const int PROFILE_DATA_REQUEST = 1003;
    public const int SEARCH_USER_BY_NICK_REQUEST = 1004;
    public const int ADD_TO_FRIENDS_REQUEST = 1005;
    public const int CHAT_MSG = 2002;
    public const int PRIVATE_MSG = 2003;
    public const int ONLINE_REQUEST = 2004;
}
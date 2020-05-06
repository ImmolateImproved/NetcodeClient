using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class OnlineManager : MonoBehaviour
{
    [SerializeField]
    private Socket socket;

    private UserData[] onlineUserDatas;

    public Dictionary<int, string> IdToNicknameMap { get; private set; } = new Dictionary<int, string>();

    public static event Action<UserData[]> OnlineChanged = delegate { };

    private void OnEnable()
    {
        socket.On(ServerEvents.ONLINE_RESPONSE, ReciveOnlineUsers);
    }

    private void OnDisable()
    {
        socket.Off(ServerEvents.ONLINE_RESPONSE, ReciveOnlineUsers);
    }

    private void ReciveOnlineUsers(NetworkMessage networkMessage)
    {
        onlineUserDatas = JsonConvert.DeserializeObject<UserData[]>(networkMessage.jsonMessage);

        OnlineChanged(onlineUserDatas);

        for (int i = 0; i < onlineUserDatas.Length; i++)
        {
            IdToNicknameMap[onlineUserDatas[i].id] = onlineUserDatas[i].nick;
        }
    }
}
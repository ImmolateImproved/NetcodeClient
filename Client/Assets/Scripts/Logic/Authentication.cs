using Newtonsoft.Json;
using UnityEngine;
using System;
using MEC;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/Authentication")]
public class Authentication : Logic
{
    private NetworkManager networkManager;

    [SerializeField]
    private string localAuthUrl = "192.168.0.105:8080";

    [SerializeField]
    private string remoteAuthUrl = "46.119.183.31:55443";

    private string login, password;

    private TokenData token;

    public event Action<bool> OnAuthentification = delegate { };
    public event Action OnTokenNotFound = delegate { };

    public override void Init()
    {
        networkManager = LogicManager.GetLogicComponent<NetworkManager>();

        Timing.CallDelayed(0.1f, TryLogin);
    }

    public override void MyOnEnable()
    {
        networkManager.On(ServerEvents.TOKEN_AUTH, AuthenticationResponseHandler);
    }

    public override void MyOnDisable()
    {
        networkManager.Off(ServerEvents.TOKEN_AUTH, AuthenticationResponseHandler);
    }

    private void AuthenticationResponseHandler(NetworkMessage networkMessage)
    {
        var result = JsonConvert.DeserializeObject<AuthenticationStatus>(networkMessage.jsonMessage);

        Debug.Log("AuthenticationStatus: " + result.status);

        var json = JsonConvert.SerializeObject(token);
        networkManager.Send(ClientEvents.PROFILE_DATA_REQUEST, json);

        if (result.status)
        {
            DataBase.Init(login);
        }

        OnAuthentification(result.status);
    }

    private void TryLogin()
    {
        //var tokenData = SaveSystem.Load<TokenData>("token");

        //if (tokenData.HasValue)
        //{
        //    token = tokenData.Value;

        //    Debug.Log("TOKEN EXISTS: " + token.token);

        //    var tokenJson = JsonConvert.SerializeObject(token);
        //    socket.Connect(tokenJson);

        //    Timing.CallDelayed(10, () =>
        //    {
        //        if (!socket.IsOpen)
        //        {
        //            OnTokenNotFound();
        //        }
        //    });
        //}
        //else
        {
            Debug.Log("TOKEN NOT FOUND");
            OnTokenNotFound();
        }
    }

    private IEnumerator<float> AuthenticationCoroutine()
    {
        var json = JsonConvert.SerializeObject(new LoginData { login = login, password = password });
        Debug.Log("LOGIN DATA: " + json);

        var url = networkManager.IsLocal ? localAuthUrl : remoteAuthUrl;
        url = NetworkManager.GetUrl(UrlType.Auth, url);

        var request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return Timing.WaitUntilDone(request.SendWebRequest());

        if (request.isNetworkError || request.isHttpError)
        {
            OnAuthentification(false);
        }
        else
        {
            token = JsonConvert.DeserializeObject<TokenData>(request.downloadHandler.text);
            SaveSystem.Save(token, "token");
            Debug.Log("request.downloadHandler.text: " + request.downloadHandler.text);

            networkManager.Connect();
            networkManager.Send(ClientEvents.TOKEN_AUTH, request.downloadHandler.text);
        }
    }

    public void Login(string login, string password)
    {
        this.login = login;
        this.password = password;
        Timing.RunCoroutine(AuthenticationCoroutine());
    }
}
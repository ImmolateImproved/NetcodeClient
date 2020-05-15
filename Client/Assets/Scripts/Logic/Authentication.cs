using Newtonsoft.Json;
using UnityEngine;
using System;
using MEC;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Authentication : MonoBehaviour
{
    [SerializeField]
    private Socket socket;

    [SerializeField]
    private string localAuthUrl = "192.168.0.105:8080";

    [SerializeField]
    private string remoteAuthUrl = "46.119.183.31:55443";

    private string login, password;

    private TokenData token;

    public static event Action<bool> OnAuthentification = delegate { };
    public static event Action OnTokenNotFound = delegate { };

    private void OnEnable()
    {
        //DataBase.ExecuteQueryWithoutAnswer(@"INSERT INTO highscores(name,score) VALUES('Joe','25');");

        //var data = DataBase.GetTable("SELECT * FROM highscores;");

        //var id = int.Parse(data.Rows[0][1].ToString());

        //Debug.Log(id);

        OnStart();

        socket.On(ServerEvents.TOKEN_AUTH, AuthenticationResponseHandler);
    }

    private void OnDisable()
    {
        socket.Off(ServerEvents.TOKEN_AUTH, AuthenticationResponseHandler);
    }

    private void AuthenticationResponseHandler(NetworkMessage networkMessage)
    {
        var result = JsonConvert.DeserializeObject<AuthenticationStatus>(networkMessage.jsonMessage);

        Debug.Log("AuthenticationStatus: " + result.status);

        var json = JsonConvert.SerializeObject(token);
        socket.Send(ClientEvents.PROFILE_DATA_REQUEST, json);

        OnAuthentification(result.status);
    }

    private void OnStart()
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

    private IEnumerator AuthenticationCoroutine()
    {
        var json = JsonConvert.SerializeObject(new LoginData { login = login, password = password });
        Debug.Log("LOGIN DATA: " + json);

        var url = socket.IsLocal ? localAuthUrl : remoteAuthUrl;
        url = socket.GetUrl(UrlType.Auth, url);

        var request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            OnAuthentification(false);
        }
        else
        {
            token = JsonConvert.DeserializeObject<TokenData>(request.downloadHandler.text);
            SaveSystem.Save(token, "token");
            Debug.Log("request.downloadHandler.text: " + request.downloadHandler.text);

            socket.Connect();
            socket.Send(ClientEvents.TOKEN_AUTH, request.downloadHandler.text);
        }
    }

    public void Login()
    {
        StartCoroutine(AuthenticationCoroutine());
    }

    public void SetLogin(string login)
    {
        this.login = login;
    }

    public void SetPassword(string password)
    {
        this.password = password;
    }
}
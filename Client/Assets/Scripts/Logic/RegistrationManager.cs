using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

struct LoginData
{
    public string login;
    public string password;
}

struct SuccesResponseData
{
    public string token;
}

struct FaildResponse
{
    public string error;
}

public class RegistrationManager : MonoBehaviour
{
    [SerializeField]
    private Socket socket;

    [SerializeField]
    private string localAuthUrl = "192.168.0.105:8080";

    [SerializeField]
    private string remoteAuthUrl = "46.119.183.31:55443";

    private string login, password;

    public static event Action OnRegistrationSucceed = delegate { };
    public static event Action OnRegistrationFaild = delegate { };

    private IEnumerator RegistrationCoroutine()
    {
        var json = JsonConvert.SerializeObject(new LoginData { login = login, password = password });

        var url = socket.IsLocal ? localAuthUrl : remoteAuthUrl;
        url = socket.GetUrl(UrlType.Reg, url);

        var request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            OnRegistrationFaild();

            Debug.Log(request.error);
        }
        else
        {
            OnRegistrationSucceed();

            Debug.Log(request.downloadHandler.text);
        }
    }

    public void Registration()
    {
        StartCoroutine(RegistrationCoroutine());
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
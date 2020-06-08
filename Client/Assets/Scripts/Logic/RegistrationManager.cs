using MEC;
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

[CreateAssetMenu(menuName = "ScriptableObjects/Logic/RegistrationManager")]
public class RegistrationManager : Logic
{
    private NetworkManager networkManager;

    [SerializeField]
    private string localAuthUrl = "192.168.0.105:8080";

    [SerializeField]
    private string remoteAuthUrl = "46.119.183.31:55443";

    [NonSerialized]
    private string login, password;

    public event Action OnRegistrationSucceed = delegate { };
    public event Action OnRegistrationFaild = delegate { };

    public override void Init()
    {
        networkManager = LogicManager.GetLogicComponent<NetworkManager>();
    }

    private IEnumerator<float> RegistrationCoroutine()
    {
        var json = JsonConvert.SerializeObject(new LoginData { login = login, password = password });

        var url = networkManager.IsLocal ? localAuthUrl : remoteAuthUrl;
        url = NetworkManager.GetUrl(UrlType.Reg, url);

        var request = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return Timing.WaitUntilDone(request.SendWebRequest());

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

    public void Registration(string login, string password)
    {
        this.login = login;
        this.password = password;

        Timing.RunCoroutine(RegistrationCoroutine());
    }
}
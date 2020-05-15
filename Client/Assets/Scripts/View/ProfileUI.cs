using UnityEngine;
using UnityEngine.UI;
using MEC;
using System.Collections.Generic;

public class ProfileUI : MonoBehaviour
{
    [SerializeField]
    private Text nicknameText;

    [SerializeField]
    private Text idText;

    private void OnEnable()
    {
        ProfileManager.OnReciveProfileData += OnReciveProfileData;
    }

    private void OnDisable()
    {
        ProfileManager.OnReciveProfileData -= OnReciveProfileData;
    }

    private void OnReciveProfileData(UserData profileData)
    {
        nicknameText.text = profileData.nick; ;
        idText.text = "Your ID: " + profileData.id.ToString(); ;
    }

}
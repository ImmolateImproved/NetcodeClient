using UnityEngine;
using UnityEngine.UI;
using MEC;
using System.Collections.Generic;

public class ProfileUI : MonoBehaviour
{
    private ProfileManager profileManager;

    [SerializeField]
    private Text nicknameText;

    [SerializeField]
    private Text idText;

    private void Awake()
    {
        profileManager = LogicManager.GetLogicComponent<ProfileManager>();
    }

    private void OnEnable()
    {
        profileManager.OnReciveProfileData += OnReciveProfileData;
    }

    private void OnDisable()
    {
        profileManager.OnReciveProfileData -= OnReciveProfileData;
    }

    private void OnReciveProfileData(UserData profileData)
    {
        nicknameText.text = profileData.nick;
        idText.text = "Your ID: " + profileData.id.ToString();
    }
}
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour
{
    [SerializeField]
    private Text nicknameText;

    [SerializeField]
    private Text idText;

    private void OnEnable()
    {
        Authentication.OnAuthentification += Authentication_OnAuthentification;
    }

    private void OnDisable()
    {
        Authentication.OnAuthentification -= Authentication_OnAuthentification;
    }

    private void Authentication_OnAuthentification(UserData userData)
    {
        nicknameText.text = userData.nick;
        idText.text = "Your ID: " + userData.id.ToString();
    }
}
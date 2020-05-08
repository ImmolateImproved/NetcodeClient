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

    private bool ready;

    private string nick, id;

    private CoroutineHandle profileUpdateHandle;

    private void OnEnable()
    {
        profileUpdateHandle = Timing.RunCoroutine(ProfileUpdate());
        Authentication.OnAuthentification += Authentication_OnAuthentification;
    }

    private void OnDisable()
    {
        Timing.KillCoroutines(profileUpdateHandle);
        Authentication.OnAuthentification -= Authentication_OnAuthentification;
    }

    private IEnumerator<float> ProfileUpdate()
    {
        while (true)
        {
            if (ready)
            {
                nicknameText.text = nick;
                idText.text = id;
                break;
            }

            yield return Timing.WaitForSeconds(1);

        }
    }

    private void Authentication_OnAuthentification(UserData userData)
    {
        nick = userData.nick;
        id = "Your ID: " + userData.id.ToString();

        ready = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineManagerUI : MonoBehaviour
{
    [SerializeField]
    private User userUIPrefab;

    [SerializeField]
    private Text onlineCountText;

    [SerializeField]
    private Transform parent;

    private List<User> usersOnlineUI = new List<User>();

    private UserData[] onlineUserDatas;

    private bool onlineHasChanged;

    private Coroutine coroutine;

    private void OnEnable()
    {
        OnlineManager.OnlineChanged += OnlineManager_OnlineChanged;
        coroutine = StartCoroutine(SpawnOnlineUsersUI());
    }

    private void OnDisable()
    {
        OnlineManager.OnlineChanged -= OnlineManager_OnlineChanged;
        StopCoroutine(coroutine);
    }

    private void OnlineManager_OnlineChanged(UserData[] userDatas)
    {
        onlineUserDatas = userDatas;
        onlineHasChanged = true;
    }

    private IEnumerator SpawnOnlineUsersUI()
    {
        while (true)
        {
            if (onlineHasChanged && onlineUserDatas != null)
            {
                onlineHasChanged = false;

                onlineCountText.text = onlineUserDatas.Length.ToString();

                for (int i = 0; i < usersOnlineUI.Count; i++)
                {
                    Destroy(usersOnlineUI[i].gameObject);
                }

                usersOnlineUI.Clear();

                for (int i = 0; i < onlineUserDatas.Length; i++)
                {
                    var user = Instantiate(userUIPrefab, parent);
                    user.Init(onlineUserDatas[i]);
                    Debug.Log("Online: " + onlineUserDatas[i].nick);
                    usersOnlineUI.Add(user);
                }
            }
            yield return null;
        }
    }
}
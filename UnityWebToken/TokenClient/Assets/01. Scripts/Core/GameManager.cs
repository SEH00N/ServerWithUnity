using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] string host;
    [SerializeField] int port;

    public static GameManager Instance;

    public string Token {get; private set;}

    private void Awake()
    {
        if(Instance != null)
            Debug.LogError("Multiple GameManager is Running");

        Instance = this;

        NetworkManager.Instance = new NetworkManager(host, port);

        Token = PlayerPrefs.GetString(LoginUI.TokenKey, string.Empty);
        if(!string.IsNullOrEmpty(Token))
        {
            NetworkManager.Instance.DoAuth();
        }
    }

    #region Debug
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            NetworkManager.Instance.GetRequest("lunch", "?date=20230704", (type, msg) => {
                if(type == MessageType.SUCCESS)
                {
                    LunchVO lunch = JsonUtility.FromJson<LunchVO>(msg);

                    foreach(string menu in lunch.menus)
                        Debug.Log(menu);
                }
                else
                {
                    Debug.Log(msg);
                }
            });
        }

    }
    #endregion

    public void DestroyToken()
    {
        PlayerPrefs.DeleteKey(LoginUI.TokenKey);
        Token = String.Empty;
    }
}

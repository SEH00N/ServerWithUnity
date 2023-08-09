using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] string host;
    [SerializeField] int port;

    public static GameManager Instance;

    private void Awake()
    {
        if(Instance != null)
            Debug.LogError("Multiple GameManager is Running");

        Instance = this;

        NetworkManager.Instance = new NetworkManager(host, port);
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
}

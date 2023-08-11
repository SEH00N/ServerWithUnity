using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public enum MessageType
{
    ERROR = 1,
    SUCCESS = 2,
    EMPTY = 3
}

public class NetworkManager
{
    public static NetworkManager Instance;
    
    private string host;
    private int port;

    public NetworkManager(string host, int port)
    {
        this.host = host;
        this.port = port;
    }

    public void GetRequest(string uri, string query, Action<MessageType, string> callback)
    {
        GameManager.Instance.StartCoroutine(GetCoroutine(uri, query, callback));
    }

    private IEnumerator GetCoroutine(string uri, string query, Action<MessageType, string> callback)
    {
        string url = $"{host}:{port}/{uri}{query}";
        UnityWebRequest req = UnityWebRequest.Get(url);

        SetRequestToken(req);
        
        yield return req.SendWebRequest();

        if(req.result != UnityWebRequest.Result.Success)
        {
            callback?.Invoke(MessageType.ERROR, $"{req.responseCode}_Error on Get");
            yield break;
        }

        MessageDTO msg = JsonUtility.FromJson<MessageDTO>(req.downloadHandler.text);
        callback?.Invoke(msg.type, msg.message);

        req.Dispose();
    }

    public void PostRequest(string uri, Payload payload, Action<MessageType, string> callback)
    {
        GameManager.Instance.StartCoroutine(PostCoroutine(uri, payload, callback));
    }

    public IEnumerator PostCoroutine(string uri, Payload payload, Action<MessageType, string> callback)
    {
        string url = $"{host}:{port}/{uri}";
        UnityWebRequest req = UnityWebRequest.Post(url, payload.GetWWWForm());
        // req.SetRequestHeader("content-type", "application/json");

        SetRequestToken(req);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            // UIController.Instance.Message.AddMessage($"요청이 실패했습니다. {req.responseCode} Error on post", 3f);
            yield break;
        }

        MessageDTO msg = JsonUtility.FromJson<MessageDTO>(req.downloadHandler.text);
        callback?.Invoke(msg.type, msg.message);

        req.Dispose();
    }

    public void DoAuth()
    {
        GetRequest("user", "", (type, json) => {
            if(type == MessageType.SUCCESS) {
                UserVO user = JsonUtility.FromJson<UserVO>(json);
                // UIController.Instance.SetLogin(user); //수업내용
                UIManager.Instance.LoginPanel.SetLogin(); // 내꺼
                Debug.Log(json);
            }
        });
    }

    private void SetRequestToken(UnityWebRequest req)
    {
        if(!string.IsNullOrEmpty(GameManager.Instance.Token))
        {
            req.SetRequestHeader("Authorization", $"Bearer{GameManager.Instance.Token}");
        }
    }
}

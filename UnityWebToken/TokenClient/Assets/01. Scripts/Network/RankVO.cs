using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RankVO : Payload
{
    public string id;
    public int level;

    public string GetJsonString()
    {
        return JsonUtility.ToJson(this);
    }

    public string GetQueryString()
    {
        return "";
    }

    public WWWForm GetWWWForm()
    {
        WWWForm form = new WWWForm();
        form.AddField("json", GetJsonString());
        return form;
    }
}
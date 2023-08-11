using System.Collections.Generic;
using UnityEngine;

public class InventoryVO : Payload
{
    public int count;
    public List<ItemVO> list;

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
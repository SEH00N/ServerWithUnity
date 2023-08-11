using System;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel
{
    private Label rankLabel;

    public MenuPanel(VisualElement root)
    {
        rankLabel = root.Q<Label>("RankLabel");

        root.Q<Button>("LogoutButton").RegisterCallback<ClickEvent>(LogoutButtonHandle);
        root.Q<Button>("RankButton").RegisterCallback<ClickEvent>(RankButtonHandle);
        root.Q<Button>("PostButton").RegisterCallback<ClickEvent>(PostButtonHandle);
    }

    private void LogoutButtonHandle(ClickEvent evt)
    {
        // 로그아웃
    }

    private void RankButtonHandle(ClickEvent evt)
    {
        // 서버에 랭킹 겟 하기
        NetworkManager.Instance.GetRequest("user/rank", "", (type, json) => {
            StringBuilder builder = new StringBuilder();
            Debug.Log(json);
            RankingVO vo = JsonUtility.FromJson<RankingVO>(json);

            for(int i = 0; i < vo.rankingList.Count; i++)
            {
                if(i != 0)
                    builder.AppendLine();
                builder.Append($"{i + 1}.\t{vo.rankingList[i].id} {vo.rankingList[i].level}레벨");
            }

            rankLabel.text = builder.ToString();
        });
    }
    
    private void PostButtonHandle(ClickEvent evt)
    {
        // 자랑하기
        RankVO vo = new RankVO() { id = "", level = UIManager.Instance.Stick.Level };
        NetworkManager.Instance.PostRequest("user/rank", vo, (type, json) => {
            if(type == MessageType.SUCCESS)
            {
                Debug.Log("성공");
            }
        });
    }
}

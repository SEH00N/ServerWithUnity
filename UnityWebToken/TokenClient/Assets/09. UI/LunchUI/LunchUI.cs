using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class LunchUI : WindowUI
{
    private TextField dateTextField;
    private Label lunchLabel;

    public LunchUI(VisualElement root) : base(root)
    {
        dateTextField = root.Q<TextField>("DateTextField");
        lunchLabel = root.Q<Label>("LunchLabel");

        root.Q<Button>("LoadButton").RegisterCallback<ClickEvent>(OnLoadButtonHandle);
        root.Q<Button>("CloseButton").RegisterCallback<ClickEvent>(OnCloseButtonHandle);
    }

    private void OnLoadButtonHandle(ClickEvent evt)
    {
        string dateStr = dateTextField.value;
        Regex regex = new Regex(@"20[0-9]{2}[0-1][0-9][0-3][0-9]");
        if(regex.IsMatch(dateStr) == false)
        {
            UIController.Instance.Message.AddMessage("똑바로 안하나", 3f);
            return;
        }

        // 무언가
        NetworkManager.Instance.GetRequest("lunch", $"?date={dateStr}", (type, msg) => {
            LunchVO lunch = JsonUtility.FromJson<LunchVO>(msg);
            StringBuilder builder = new StringBuilder();

            foreach(string menu in lunch.menus)
                builder.Append($"{menu}\n");


            lunchLabel.text = builder.ToString();
        });

        // string menuStr = "아무튼 메뉴";
    }
    
    private void OnCloseButtonHandle(ClickEvent evt)
    {
        // Do nothing
        Close();
    }
}

using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UserInfoPanel
{
    private VisualElement root;
    private Button infoButton;

    public UserPopOver UserPopOver { get; private set; }

    private UserVO user;
    public UserVO User {
        get => user;
        set {
            user = value;

            infoButton.text = user.name;
            UserPopOver.UserName = user.name;
            UserPopOver.Email = user.email;
            UserPopOver.Exp = user.exp;
        }
    }

    public UserInfoPanel(VisualElement root, VisualElement popOverElement, EventCallback<ClickEvent> logoutHandler)
    {
        this.root = root;
        infoButton = this.root.Q<Button>("InfoButton");

        root.Q<Button>("LogoutButton").RegisterCallback<ClickEvent>(logoutHandler);

        UserPopOver = new UserPopOver(popOverElement);
        infoButton.RegisterCallback<MouseEnterEvent>(e => {
            Rect rect = infoButton.worldBound;
            Vector2 pos = rect.position;
            pos.y += rect.height + 10;

            UserPopOver.PopOver(pos);
        });

        infoButton.RegisterCallback<MouseLeaveEvent>(e => {
            UserPopOver.Hide();
        });
    }

    public void Show(bool value)
    {
        if(value)
           root.RemoveFromClassList("widthzero");
        else
            root.AddToClassList("widthzero");
    }
}

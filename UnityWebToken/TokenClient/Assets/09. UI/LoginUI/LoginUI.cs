using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginUI : WindowUI
{
    private TextField emailField;
    private TextField passwordField;

    public LoginUI(VisualElement root) : base(root)
    {
        emailField = root.Q<TextField>("EmailInput");
        passwordField = root.Q<TextField>("PasswordInput");

        root.Q<Button>("OkButton").RegisterCallback<ClickEvent>(OnLoginButtonHandle);
        root.Q<Button>("CancelButton").RegisterCallback<ClickEvent>(OnCancelButtonHandle);
    }

    public override void Open()
    {
        if(emailField != null)
            emailField.value = "";
        if(passwordField != null)
            passwordField.value = "";

        base.Open();
    }

    private void OnLoginButtonHandle(ClickEvent evt)
    {
        LoginDTO loginDTO = new LoginDTO() { email = emailField.value, password = passwordField.value };
        NetworkManager.Instance.PostRequest("user/login", loginDTO, (type, json) => {
            Debug.Log(type);
            Debug.Log(json);
        });
    }

    private void OnCancelButtonHandle(ClickEvent evt)
    {
        Close();
    }
}

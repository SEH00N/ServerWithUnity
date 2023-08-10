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


    public const string TokenKey = "token";
    private void OnLoginButtonHandle(ClickEvent evt)
    {
        LoginDTO loginDTO = new LoginDTO() { email = emailField.value, password = passwordField.value };
        NetworkManager.Instance.PostRequest("user/login", loginDTO, (type, json) => {
            if(type == MessageType.SUCCESS)
            {
                // UserVO vo = JsonUtility.FromJson<UserVO>(json);
                TokenResponseDTO dto = JsonUtility.FromJson<TokenResponseDTO>(json);
                PlayerPrefs.SetString(TokenKey, dto.token);
                
                UIController.Instance.SetLogin(dto.user);
                Close();
            }
            else
            {
                UIController.Instance.Message.AddMessage(json, 3f);
            }
        });
    }

    private void OnCancelButtonHandle(ClickEvent evt)
    {
        Close();
    }
}

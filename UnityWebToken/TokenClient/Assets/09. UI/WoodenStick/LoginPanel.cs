using UnityEngine;
using UnityEngine.UIElements;

public class LoginPanel
{
    #region login
    private TextField idField;
    private TextField pwField;

    private VisualElement loginPanel;
    #endregion


    public const string TokenKey = "token";

    public LoginPanel(VisualElement root)
    {
        idField = root.Q<TextField>("IDField");
        pwField = root.Q<TextField>("PWField");
        loginPanel = root.Q<VisualElement>("LoginPanel");

        root.Q<Button>("LoginButton").RegisterCallback<ClickEvent>(OnLoginClickHandle);
        root.Q<Button>("LogoutButton").RegisterCallback<ClickEvent>(OnLogoutClickHandle);
    }

    private void OnLogoutClickHandle(ClickEvent evt)
    {
        SetLogout();
    }

    private void OnLoginClickHandle(ClickEvent evt)
    {
        if(string.IsNullOrEmpty(idField.value) || string.IsNullOrEmpty(pwField.value))
        {
            Debug.Log("똑바로 입력 안하나");
            return;
        }

        LoginDTO loginDTO = new LoginDTO() { email = idField.value, password = pwField.value };
        NetworkManager.Instance.PostRequest("user/login", loginDTO, (type, json) => {
            if(type == MessageType.SUCCESS)
            {
                // UserVO vo = JsonUtility.FromJson<UserVO>(json);
                TokenResponseDTO dto = JsonUtility.FromJson<TokenResponseDTO>(json);
                PlayerPrefs.SetString(TokenKey, dto.token);

                SetLogin();
            }
            else
            {
                Debug.Log("로그인 실패");
            }
        });
    }

    public void SetLogin()
    {
        GameManager.Instance.IsLoggedIn = true;
        loginPanel.AddToClassList("disable");
    }

    public void SetLogout()
    {
        loginPanel.RemoveFromClassList("disable");

        GameManager.Instance.IsLoggedIn = false;
        GameManager.Instance.DestroyToken();
    }
}

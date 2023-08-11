using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private UIDocument document = null;

    private WoodenStick stick;
    public WoodenStick Stick => stick;
    private LoginPanel loginPanel;
    public LoginPanel LoginPanel => loginPanel;
    private MenuPanel menuPanel;
    public MenuPanel MenuPanel => menuPanel;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple UI Controller Instance is Running");

        Instance = this;

        document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        stick = new WoodenStick(document.rootVisualElement);
        loginPanel = new LoginPanel(document.rootVisualElement);
        menuPanel = new MenuPanel(document.rootVisualElement);
    }
}

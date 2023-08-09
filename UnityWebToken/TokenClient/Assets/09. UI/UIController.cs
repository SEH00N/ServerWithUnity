using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Windows {
    Lunch = 1,
    Login
}

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] VisualTreeAsset lunchUIAsset;
    [SerializeField] VisualTreeAsset loginUIAsset;

    private UIDocument uiDocument;
    private VisualElement contentParent;

    private MessageSystem messageSystem;
    public MessageSystem Message => messageSystem;

    private Dictionary<Windows, WindowUI> windowDictionary = new Dictionary<Windows, WindowUI>();
    private LunchUI lunchUI;

    private void Awake()
    {
        if(Instance != null)
            Debug.LogError("Multiple UI Controller Instance is Running");
        
        Instance = this;

        uiDocument = GetComponent<UIDocument>();
        messageSystem = GetComponent<MessageSystem>();
    }

    private void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;

        Button lunchButton = root.Q<Button>("LunchButton");
        lunchButton.RegisterCallback<ClickEvent>(OnOpenLunchHandle);

        Button loginButton = root.Q<Button>("LoginButton");
        loginButton.RegisterCallback<ClickEvent>(OnOpenLoginHandle);

        contentParent = root.Q<VisualElement>("Content");


        VisualElement messageContainer = root.Q<VisualElement>("MessageContainer");
        messageSystem.SetContainer(messageContainer);


        windowDictionary.Clear();

        VisualElement lunchRoot = lunchUIAsset.Instantiate().Q<VisualElement>("LunchContainer");
        contentParent.Add(lunchRoot);
        LunchUI lunchUI = new LunchUI(lunchRoot);
        lunchUI.Close();
        windowDictionary.Add(Windows.Lunch, lunchUI);

        VisualElement loginRoot = loginUIAsset.Instantiate().Q<VisualElement>("LoginWindow");
        contentParent.Add(loginRoot);
        LoginUI loginUI = new LoginUI(loginRoot);
        loginUI.Close();
        windowDictionary.Add(Windows.Login, loginUI);
    }

    private void OnOpenLunchHandle(ClickEvent evt)
    {
        foreach(KeyValuePair<Windows, WindowUI> element in windowDictionary)
            element.Value.Close();

        // foreach(WindowUI ui in windowDictionary.Values)
        //     ui.Close();

        windowDictionary[Windows.Lunch].Open();
    }

    private void OnOpenLoginHandle(ClickEvent evt)
    {
        foreach (KeyValuePair<Windows, WindowUI> element in windowDictionary)
            element.Value.Close();

        windowDictionary[Windows.Login].Open();
    }
}

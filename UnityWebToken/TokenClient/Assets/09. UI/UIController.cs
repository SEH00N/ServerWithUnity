using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public enum Windows {
    Lunch = 1,
    Login,
    Inven
}

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public List<ItemSO> itemList;

    [SerializeField] VisualTreeAsset lunchUIAsset;
    [SerializeField] VisualTreeAsset loginUIAsset;
    [SerializeField] VisualTreeAsset invenUIAsset;
    [SerializeField] VisualTreeAsset itemTemplate;

    private UIDocument uiDocument;
    private VisualElement contentParent;

    private Button loginButton;

    private MessageSystem messageSystem;
    public MessageSystem Message => messageSystem;

    private Dictionary<Windows, WindowUI> windowDictionary = new Dictionary<Windows, WindowUI>();
    private LunchUI lunchUI;
    private UserInfoPanel userInfoPanel;

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

        loginButton = root.Q<Button>("LoginButton");
        loginButton.RegisterCallback<ClickEvent>(OnOpenLoginHandle);
        contentParent = root.Q<VisualElement>("Content");

        VisualElement userPopOverElement = root.Q<VisualElement>("UserPopOver");

        VisualElement userInfoElement = root.Q<VisualElement>("UserInfoPanel");
        userInfoPanel = new UserInfoPanel(userInfoElement, userPopOverElement, e => SetLogout());

        VisualElement messageContainer = root.Q<VisualElement>("MessageContainer");
        messageSystem.SetContainer(messageContainer);

        root.Q<Button>("InventoryButton").RegisterCallback<ClickEvent>(OnOpenInvenHandle);

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

        VisualElement invenRoot = invenUIAsset.Instantiate().Q<VisualElement>("InventoryBody");
        contentParent.Add(invenRoot);
        InventoryUI invenUI = new InventoryUI(invenRoot, itemTemplate);
        invenUI.Close();
        windowDictionary.Add(Windows.Inven, invenUI);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            int idx = Random.Range(0, itemList.Count);

            InventoryUI inven = windowDictionary[Windows.Inven] as InventoryUI;
            inven.AddItem(itemList[idx], 3);
        }

    }

    private void OnOpenInvenHandle(ClickEvent evt)
    {
        foreach (KeyValuePair<Windows, WindowUI> element in windowDictionary)
            element.Value.Close();

        // foreach(WindowUI ui in windowDictionary.Values)
        //     ui.Close();

        windowDictionary[Windows.Inven].Open();
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

    public void SetLogin(UserVO user)
    {
        loginButton.style.display = DisplayStyle.None;
        // userInfoPanel.RemoveFromClassList("widthzero");
        userInfoPanel.Show(true);
        userInfoPanel.User = user;
    }

    public void SetLogout()
    {
        loginButton.style.display = DisplayStyle.Flex;
        userInfoPanel.Show(false);

        GameManager.Instance.DestroyToken();
    }
}

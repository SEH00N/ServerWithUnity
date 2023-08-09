using UnityEngine;
using UnityEngine.UIElements;

public class MessageTemplate
{
    private VisualElement root;
    public VisualElement Root => root;
    
    private Label label;
    private float timer = 0f;
    private float currentTimer = 0f;
    private bool fade = false;
    private bool isComplete = false;
    public bool IsComplete => isComplete;

    public string Text {
        get => label.text;
        set => label.text = value;
    }

    public MessageTemplate(VisualElement root, float timer)
    {
        this.root = root;
        label = root.Q<Label>("Message");

        currentTimer = 0f;
        fade = false;
        this.timer = timer;
    }

    public void UpdateMessage()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer >= timer && !fade)
        {
            root.AddToClassList("off");
            fade = true;
        }

        if(currentTimer >= timer + 0.6f)
            isComplete = true;
    }
}

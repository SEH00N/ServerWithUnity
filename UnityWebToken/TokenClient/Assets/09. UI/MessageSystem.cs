using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageSystem : MonoBehaviour
{
    [SerializeField] VisualTreeAsset messageTemplate;
    private VisualElement container;
    private List<MessageTemplate> messageList = new List<MessageTemplate>();

    public void SetContainer(VisualElement container)
    {
        this.container = container;
    }

    private void Update()
    {
        for(int i = 0; i < messageList.Count; i++)
        {
            messageList[i].UpdateMessage();
            if(messageList[i].IsComplete)
            {
                messageList[i].Root.RemoveFromHierarchy();
                messageList.RemoveAt(i);
                --i;
            }
        }
    }

    public void AddMessage(string text, float timer)
    {
        VisualElement msgElement = messageTemplate.Instantiate().Q<VisualElement>("MessageBox");
        container.Add(msgElement);

        MessageTemplate msg = new MessageTemplate(msgElement, timer);
        msg.Text = text;

        messageList.Add(msg);
    }
}

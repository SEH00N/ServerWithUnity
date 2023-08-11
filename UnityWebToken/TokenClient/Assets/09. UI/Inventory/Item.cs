using UnityEngine;
using UnityEngine.UIElements;

public class Item
{
    public ItemSO dataSO;
    public int slotNumber;
    
    private VisualElement root;
    private VisualElement sprite;
    private int count;
    private Label countLabel;

    public int Count {
        get => count;
        set {
            count = value;
            countLabel.text = count.ToString();
        }
    }

    public Item(VisualElement root, ItemSO data, int slotNumber, int count)
    {
        this.root = root;
        this.slotNumber = slotNumber;
        dataSO = data;
        
        sprite = root.Q<VisualElement>("Sprite");
        countLabel = root.Q<Label>("CountLabel");

        sprite.style.backgroundImage = new StyleBackground(dataSO.sprite);
        Count = count;
    }
}

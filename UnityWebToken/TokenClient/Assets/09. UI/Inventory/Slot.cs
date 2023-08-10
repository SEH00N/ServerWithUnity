using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot
{
    private VisualElement root;
    public int slotNumber;

    public VisualElement Root => root;
    public VisualElement Child {
        get => root.childCount == 0 ? null : root.Children().First();
    }

    public Slot(VisualElement root, int id)
    {
        this.root = root;
        slotNumber = id;
    }
}

using System;
using UnityEngine.UIElements;

public class WindowUI
{
    protected VisualElement root;

    public WindowUI(VisualElement root)
    {
        this.root = root;
    }

    public virtual void Open()
    {
        root.RemoveFromClassList("fade");
    }

    public virtual void Close()
    {
        root.AddToClassList("fade");
    }
}

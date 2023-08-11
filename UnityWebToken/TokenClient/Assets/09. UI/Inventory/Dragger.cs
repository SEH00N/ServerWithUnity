using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Dragger : MouseManipulator
{
    private Action<MouseUpEvent, VisualElement, VisualElement> dropCallback;

    private bool isDrag = false;
    private Vector2 startPos;
    private VisualElement beforeSlot;

    public Dragger(Action<MouseUpEvent, VisualElement, VisualElement> dropCallback)
    {
        activators.Add(new ManipulatorActivationFilter(){button = MouseButton.LeftMouse});
        this.dropCallback = dropCallback;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected void OnMouseDown(MouseDownEvent evt)
    {
        if (!CanStartManipulation(evt))
            return;

        float x = target.layout.x;
        float y = target.layout.y;

        beforeSlot = target.parent;
        VisualElement container = target.parent.parent;

        target.RemoveFromHierarchy();
        container.Add(target);

        isDrag = true;
        target.CaptureMouse();
        startPos = evt.localMousePosition;

        Vector2 offset = evt.mousePosition - container.worldBound.position - startPos;

        target.style.position = Position.Absolute;
        target.style.left = offset.x;
        target.style.top = offset.y;
    }

    protected void OnMouseMove(MouseMoveEvent evt)
    {
        if(!isDrag || !CanStartManipulation(evt) || !target.HasMouseCapture())
            return;

        Vector2 diff = evt.localMousePosition - startPos;
        float x = target.layout.x;
        float y = target.layout.y;

        target.style.left = x + diff.x;
        target.style.top = y + diff.y;
    }

    protected void OnMouseUp(MouseUpEvent evt)
    {
        if(!isDrag || !target.HasMouseCapture())
            return;

        isDrag = false;
        target.ReleaseMouse();

        target.style.position = Position.Relative;
        target.style.left = 0;
        target.style.top = 0;

        dropCallback?.Invoke(evt, target, beforeSlot);
    }
}

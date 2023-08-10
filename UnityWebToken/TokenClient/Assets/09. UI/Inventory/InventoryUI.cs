using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : WindowUI
{
    private List<Slot> slotList = new List<Slot>();
    private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

    private VisualTreeAsset itemTemplate;

    public InventoryUI(VisualElement root, VisualTreeAsset itemTemplate) : base(root)
    {
        this.itemTemplate = itemTemplate;
        slotList = root.Query<VisualElement>(className:"slot").ToList().Select((elem, idx) => new Slot(elem, idx)).ToList();
        
        #region Test
        VisualElement item = root.Q<VisualElement>(className:"item");
        VisualElement slot = item.parent.parent;
        item.parent.RemoveFromHierarchy();

        slot.Add(item);

        item.AddManipulator(new Dragger((evt, target, beforeSlot) => {
            Debug.Log("Drop");
        }));
        #endregion
    }
}

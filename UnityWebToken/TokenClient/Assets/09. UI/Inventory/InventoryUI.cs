using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : WindowUI
{
    private List<Slot> slotList = new List<Slot>();
    private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

    private VisualTreeAsset itemTemplate;
    private bool isLoadedFromServer = false;

    public InventoryUI(VisualElement root, VisualTreeAsset itemTemplate) : base(root)
    {
        this.itemTemplate = itemTemplate;
        slotList = root.Query<VisualElement>(className:"slot").ToList().Select((elem, idx) => new Slot(elem, idx)).ToList();
    }

    public void AddItem(ItemSO dataSO, int count, int slotNumber = -1)
    {
        if(itemDictionary.TryGetValue(dataSO.itemCode, out Item findItem))
        {
            findItem.Count += count;
            return;
        }

        VisualElement itemElement = itemTemplate.Instantiate().Q<VisualElement>("Item");
        Slot emptySlot;

        if(slotNumber < 0)
        {
            emptySlot = FindEmptySlot();
            if(emptySlot == null)
            {
                UIController.Instance.Message.AddMessage("인벤토리에 빈 칸이 없어요", 3f);
                return;
            }
        }
        else
        {
            emptySlot = FindSlotByNumber(slotNumber);
        }

        emptySlot.Root.Add(itemElement);

        Item item = new Item(itemElement, dataSO, emptySlot.slotNumber, count);
        itemDictionary.Add(dataSO.itemCode, item);


        itemElement.AddManipulator(new Dragger((evt, target, beforeSlot) => {
            Slot slot = FindSlot(evt.mousePosition);
            target.RemoveFromHierarchy();

            if (slot == null)
                beforeSlot.Add(target);
            else
            {
                item.slotNumber = slot.slotNumber;
                slot.Root.Add(target);
            }
        }));
    }

    public void SaveToDB()
    {
        List<ItemVO> voList = itemDictionary.Values.Select(item => 
            new ItemVO{itemCode = item.dataSO.itemCode, count = item.Count, slotNumber = item.slotNumber}).ToList();

        InventoryVO invenVO = new InventoryVO(){list = voList, count = slotList.Count};

        if(NetworkManager.Instance == null)
            return;

        NetworkManager.Instance.PostRequest("inven", invenVO, (type, msg) => {
            if(type == MessageType.ERROR)
            {
                UIController.Instance.Message.AddMessage(msg, 3f);
            }
        });
    }

    private Slot FindEmptySlot() => slotList.Find(x => x.Child == null);
    private Slot FindSlotByNumber(int slotNumber) => slotList[slotNumber];

    private Slot FindSlot(Vector2 pos) => slotList.Find(s => s.Root.worldBound.Contains(pos));

    public override void Open()
    {
        if(NetworkManager.Instance == null)
            return;

        // foreach(Item item in itemDictionary.Values)
        //     slotList[item.slotNumber].Child.RemoveFromHierarchy();
        // itemDictionary.Clear();

        itemDictionary.Clear();
        slotList.ForEach(slot => slot.Root.Clear());

        NetworkManager.Instance.GetRequest("inven", "", (type, json) => {
            if (type == MessageType.ERROR)
            {
                UIController.Instance.Message.AddMessage(json, 3f);
                return;
            }
            if(type == MessageType.SUCCESS)
            {
                InventoryVO vo = JsonUtility.FromJson<InventoryVO>(json);
                foreach(ItemVO item in vo.list)
                {
                    ItemSO itemData = UIController.Instance.itemList.Find(i => i.itemCode == item.itemCode);
                    if(itemData != null)
                        AddItem(itemData, item.count, item.slotNumber);
                }
                    
                UIController.Instance.Message.AddMessage("인벤토리 로드 성공", 3f);
            }

            isLoadedFromServer = true;
        });

        base.Open();
    }

    public override void Close()
    {
        if(root.ClassListContains("fade") == false && isLoadedFromServer)
        {
            SaveToDB();
        }

        base.Close();

    }
}

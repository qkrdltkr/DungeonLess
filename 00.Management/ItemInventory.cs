using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : Singleton<ItemInventory> {
    public static ItemInventory Instance {
        get { return (ItemInventory)_Instance; }
        set { _Instance = value; }
    }

    private UIManager _uiManager;

    private void Start() {
        _uiManager = GameObject.Find("Managers/UIManager").GetComponent<UIManager>();
    
        // 시작 마석
        AddStone(10);
        AddRepairTool(10);
        AddMoney(1000);
    }

    public int Money { get; private set; }
    public int MagicStone { get; private set; } 
    public int RepairTool { get; private set; }

    public void AddMoney(int amount) {
        if (Money + amount > 9999) { Money = 9999;  return; }
        if (Money < 0) { Money = 0; return; }
        Money += amount;

        _uiManager.UpdateMoney(Money);
    }
    public void AddStone(int amount) {
        if (MagicStone > 99) { MagicStone = 99; return; }
        if (MagicStone < 0) { MagicStone = 0; return; }
        MagicStone += amount;

        _uiManager.UpdateMagicStone(MagicStone);
    }
    public void AddRepairTool(int amount) {
        if (RepairTool > 99) { RepairTool = 99; return; }
        if (RepairTool < 0) { RepairTool = 0; return; }
        RepairTool += amount;

       _uiManager.UpdateRepair(RepairTool);
    }


    public List<Item> ItemList { get { return _itemInventory; } private set { _itemInventory = value; } }
    private List<Item> _itemInventory = new List<Item>();
    public void Add(Item item) {
        _itemInventory.Add(item);
        item.transform.parent = this.transform;
    }
    public void Remove(int index) {
        if (index > _itemInventory.Count - 1 || index < 0) return;

        Item item = _itemInventory[index];
        _itemInventory.RemoveAt(index);
        Destroy(item);
    }
    public void Remove(Item item) {
        if (!item) return;

        _itemInventory.Remove(item);
        Destroy(item);
    }
}

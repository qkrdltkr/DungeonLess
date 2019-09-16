using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapInventory : Singleton<TrapInventory> {
    public static TrapInventory Instance
    {
        get { return (TrapInventory)_Instance; }
        set { _Instance = value; }
    }

    public List<GameObject> TrapList { get { return _trapInventory; } private set { _trapInventory = value; } }
    private List<GameObject> _trapInventory = new List<GameObject>();

    public int MaximumTrap { get; set; }

    void Start () {
        MaximumTrap = 20;
    }

    public bool Add(GameObject trap) {
        if (_trapInventory.Count > MaximumTrap - 1) return false;

        _trapInventory.Add(trap);
        return true;
    }

    public void Remove(int index) {
        if (index > _trapInventory.Count - 1 || index < 0) return;

        GameObject trap = _trapInventory[index];
        _trapInventory.RemoveAt(index);
        trap.transform.parent = null;
    }

    public void Remove(GameObject trap) {
        if (!trap) return;

        _trapInventory.Remove(trap);
        trap.transform.parent = null;
    }
}

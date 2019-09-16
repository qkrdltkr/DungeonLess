using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInventory : Singleton<MonsterInventory> {
    public static MonsterInventory Instance {
        get { return (MonsterInventory)_Instance; }
        set { _Instance = value; }
    }

    public List<GameObject> MonsterList { get { return _monsterInventory; } private set { _monsterInventory = value; } }
    private List<GameObject> _monsterInventory = new List<GameObject>();

    public int MaximumMonster { get; set; }

    public void Start() {       
        MaximumMonster = 48;  
    }

    public bool Add(GameObject monster) {
        if (_monsterInventory.Count > MaximumMonster - 1) return false;

        _monsterInventory.Add(monster);
        monster.transform.parent = this.transform;
        monster.transform.position = Vector3.one * 1000.0f;
        monster.transform.Find("HitBox").GetComponent<Monster>().Clear();  // AI 리셋
        monster.transform.Find("HitBox").GetComponent<Monster>().enabled = true;
        monster.transform.Find("HitBox").GetComponent<Collider>().enabled = false; // 충돌 방지
        return true;
    }

    public  void Remove(int index) {
        if (index > _monsterInventory.Count - 1 || index < 0) return;

        GameObject monster = _monsterInventory[index];
        _monsterInventory.RemoveAt(index);
        monster.transform.parent = null;
    }

    public  void Remove(GameObject monster) {
        if (!monster) return;

        _monsterInventory.Remove(monster);
        monster.transform.parent = null;
    }
}

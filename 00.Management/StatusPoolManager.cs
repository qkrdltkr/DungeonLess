using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPoolManager : Singleton<StatusPoolManager> { 
    public static StatusPoolManager Instance {
        get { return (StatusPoolManager)_Instance; }
        set { _Instance = value; }
    }

    public GameObject[] _StatusEffetsPrefabs;

    private List<GameObject> StatusEffets = new List<GameObject>();
    private GameObject _child;
    private GameObject _poolChild;

    private void Start() {
        Transform Parent = this.transform;
        for (int i = 0; i < Parent.childCount; ++i)  {
            FullPool(i, Parent.GetChild(i));
        }
    }

    private void FullPool(int kind, Transform parent) {
        for (int i = 0; i < 10; i++) {
            _poolChild = Instantiate(_StatusEffetsPrefabs[kind]);
            switch (kind) {
                case StatusEffectKind.kBurning: _poolChild.name = "Burning"; break;
                case StatusEffectKind.kStun: _poolChild.name = "Stun"; break;
                case StatusEffectKind.kSlow: _poolChild.name = "Slow"; break;
            }
            _poolChild.SetActive(true);
            _poolChild.transform.parent = parent;
        }
    }

    public void RechargeStatusEffect(GameObject StatusEffect, Transform kind) {
        StatusEffect.transform.parent = kind;
        StatusEffets.Add(StatusEffect);
        StatusEffect.SetActive(false);
    }
    public void SetStatusEffect(GameObject StatusEffect, Transform kind) {
        StatusEffect.transform.parent = kind;
        StatusEffets.Add(StatusEffect);
    }
    public GameObject PopStatusEffect(int kind) {
        int statusEffectIndex = 0;

        for (int j = 0; j < StatusEffets.Count; j++) {
            if (StatusEffets[j].GetComponent<StatusEffect>().Kind != kind) statusEffectIndex++;
            else j = StatusEffets.Count;
        }
        
        GameObject statusEffect;

        if (statusEffectIndex == StatusEffets.Count) statusEffect = SupplyStatusEffect(kind);
        else statusEffect = StatusEffets[statusEffectIndex];
        StatusEffets.RemoveAt(statusEffectIndex);
        return statusEffect;
    }
    private GameObject SupplyStatusEffect(int kind) {
        GameObject statusEffect = Instantiate(_StatusEffetsPrefabs[kind], this.transform.position, Quaternion.identity, this.transform);
        StatusEffets.Add(statusEffect);
        return statusEffect;
    }
}

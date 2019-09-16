using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapKind { NONPASS = -1, TEMP }
// _TrapPrefabs와 동기화 필요

public class TrapManager : MonoBehaviour {
    public GameObject[] _TrapPrefabs;
    private GameObject[] _trapSpawners;

    public void Initialize() {
        _trapSpawners = GameObject.FindGameObjectsWithTag("TrapSpawnPos");
    }
    public void AllTrapSpawn() {
        for (int i = 0; i < _trapSpawners.Length; ++i) 
            _trapSpawners[i].GetComponent<TrapSpawner>().Spawn(_TrapPrefabs);
    }
}

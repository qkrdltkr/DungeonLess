using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterKind { NONPASS = -1, SLIME, SKELETONWARRIOR ,ICE_ELEMENTAL, KINGSLIME } // 킹슬라임 임시
//public enum BossMonsterKind { NONPASS = -1, KINGSLIME }
//몬스터매니저 프리팹과 동기화 해야한다.
//네크로노미콘 인벤토리에 러프 몬스터 이미지 동기화 해줘야한다,.
public enum MonsterTribe { NONPASS = -1, SLIME, UNDEAD, DEMI_HUMAN, BEAST_HUMAN, MAGICAL_MACHINE, BEAST, DEMON, ELF}
public class MonsterManager : MonoBehaviour {
    public GameObject[] _MonsterPrefabs;           // 모든 몬스터 종류, MonsterKind와 동기화 필요
    private GameObject[] _monsterSpawners;         // 모든 스포너
    public ElementalAttributes DungeonType { get; set; }
    private void Initialize() {
        _monsterSpawners = GameObject.FindGameObjectsWithTag("MonsterSpawnPos");
        MonsterSpawner monsterSpawner;
        for (int i = 0; i < _monsterSpawners.Length; i++) {
            monsterSpawner = _monsterSpawners[i].GetComponent<MonsterSpawner>();
            monsterSpawner.Initialize();
        }
    }

    private void SetMonster(Transform player) {
        MonsterSpawner monsterSpawner = null;
        // 스폰
        for (int i = 0; i < _monsterSpawners.Length; i++){
            //조건
            monsterSpawner = _monsterSpawners[i].GetComponent<MonsterSpawner>();
            MonsterKind[] spawnKinds = monsterSpawner._SpawnKind;
  
            List<GameObject[]> spawnMonsters = new List<GameObject[]>();
            for (int j = 0; j < spawnKinds.Length; ++j) {
                MonsterKind kind = MonsterKind.NONPASS;
                switch (spawnKinds[j]) {
                    case MonsterKind.SLIME:           kind = MonsterKind.SLIME;           break;
                    case MonsterKind.SKELETONWARRIOR: kind = MonsterKind.SKELETONWARRIOR; break;
                    case MonsterKind.ICE_ELEMENTAL:   kind = MonsterKind.ICE_ELEMENTAL;   break;
                    case MonsterKind.KINGSLIME:       kind = MonsterKind.KINGSLIME;       break;
                }
                spawnMonsters.Add(Utility.ConvertList2Array(Sort(kind)));
            }
          
            monsterSpawner.SetSpawnMonsters(spawnMonsters);
            monsterSpawner.Spawn(player);              
        }
    }
    private List<GameObject> Sort(MonsterKind kind) {
        // 몬스터 찾아서 넣기
        List<GameObject> monsters = new List<GameObject>();
        for(int i = 0; i < _MonsterPrefabs.Length; ++i) {
            Monster monster = _MonsterPrefabs[i].GetComponentInChildren<Monster>();
            if (monster._Kind == kind && monster._Attributes == DungeonType) {
                monsters.Add(_MonsterPrefabs[i]);
            }
        }
        return monsters;
    }

public void AllMonsterSpawn(Transform player) {
        Initialize();
        SetMonster(player);
    }
}

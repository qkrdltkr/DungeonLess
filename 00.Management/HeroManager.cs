using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroKind  { NONPASS = -1,
    VILLIGER_WARRIOR, VILLIGER_ARCHER,           // 0 ~ 1
    ADVENTURER_WARRIOR, ADVENTURER_ARCHER        // 2 ~ 3
}
//HeroManager 프리팹과 동기화 해야한다, GetRank 수정도 해야됨

public enum HeroRank { NONPASS = -1, VILLIGER, ADVENTURER}

public struct HeroSpawnDescription {
    public HeroRank[] Ranks { get; set; }
    public int Population   { get; set; }

    public HeroSpawnDescription(HeroRank[] ranks, int population) {
        Ranks = ranks;
        Population = population;
    }
}

public class HeroParty {
    public string Name { get; set; }
    public GameObject Leader { get; set; }
    private List<GameObject> partyMembers = new List<GameObject>();
    public List<GameObject> PartyMembers { get { return partyMembers; } set { partyMembers = value; } }
}
public class HeroManager : MonoBehaviour {
    public GameObject[] _HeroPrefabs;           // 모든 몬스터 종류, HeroKind 동기화 필요
    private HeroSpawnDescription[] _heroSpawnDescriptions;
    private Transform _entrance;
    private static List<HeroParty>  _heroParties = new List<HeroParty>();
    private static List<GameObject> _heros = new List<GameObject>();

    public static List<HeroParty> HeroParies { get { return _heroParties; } set { _heroParties = value; } }
    public static List<GameObject> Heros { get { return _heros; } set { _heros = value; } }


    private void Start() {
        CreateHeroSpawnDescription();
    }

    public void Init() {
        _entrance = GameObject.FindGameObjectWithTag("Entrance").transform;
    }

    public void Pop() {
        HeroSpawnDescription description = _heroSpawnDescriptions[DungeonMaster.Stage];
        List<GameObject> spawnList = new List<GameObject>();

        int margin = description.Population;
        while (margin > 0) { // 0. 인구수 맞춰질때까지 반복            
            // 0-1. 내 랭크 내에서 무작위로 뽑을 랭크 선정
            HeroRank rank = description.Ranks[Random.Range(0, description.Ranks.Length)];
            // 0-2. 그 랭크에 맞는 영웅 리스트에 추가
            GameObject hero = GetHero(rank);
            spawnList.Add(hero);
            // 0-3. 남은 인구수 파악 : 인구수 1 → 빌리지, 2 → 모험가
            margin -= (int)rank + 1;
        }
        // 1. 초과된 인구수 구하기
        margin *= -1;
        // 2. 앞에서부터 영웅 가져와서 인구수 비교, 영웅 인구수가 더 적을 경우 리스트에서 제외하고 초과된 인구수 다시 연산
        while (margin > 0) {
            for(int i = 0; i < spawnList.Count; ++i) {
                int population = (int)spawnList[i].GetComponentInChildren<Hero>()._Rank + 1;
                if (population <= margin) {
                    spawnList.RemoveAt(i);
                    margin -= population;
                    break;
                }
            }
        }
        // 3. 스폰
        Spawn(spawnList);
        PartyMatching();
        SetLeader();
        SetPartyName();
    }

    private GameObject GetHero(HeroRank rank) {
        int start = -1; int end = -1;
        switch (rank) {
            case HeroRank.VILLIGER:     start = (int)HeroKind.VILLIGER_WARRIOR;   end = (int)HeroKind.VILLIGER_ARCHER;     break;
            case HeroRank.ADVENTURER:   start = (int)HeroKind.ADVENTURER_WARRIOR; end = (int)HeroKind.ADVENTURER_ARCHER;   break;
        }

        return _HeroPrefabs[Random.Range(start, end + 1)] ;
    }

    private void Spawn(List<GameObject> spawnList) {
        _entrance = GameObject.FindGameObjectWithTag("Entrance").transform;
        Debug.Log(_entrance.name);
        for (int i = 0; i < spawnList.Count; ++i) {
            GameObject go = Instantiate(spawnList[i], new Vector3(_entrance.position.x, _entrance.position.y,
                    ObjectSortLevel.kCharacter), Quaternion.identity);
            go.GetComponentInChildren<Character>().Index.Set(_entrance.GetComponentInParent<Room>().Index);
            _heros.Add(go);
        }
    }

    private void PartyMatching() {
        int cnt = _heros.Count;
        int listIndex = 0;
        while (cnt > 0) {
            // 4-0. 남은 인원수를 조사한다. 
            // 4-1. 4인 이상일 경우 1인 10% 2인 20% 3인 30% 4인 40%
            if (cnt >= 4)  {
                int party = Utility.RandomIndexWithProbability(new int[] { 10, 20, 30, 40 }) + 1;
                HeroParty hp = new HeroParty();
                for (int i = 0; i < party; ++i) {
                    hp.PartyMembers.Add(_heros[listIndex++]);
                }
                _heroParties.Add(hp);
                cnt -= party;
            }
            // 4-2. 3인 일 경우     1인 20% 2인 30% 3인 50%
            else if (cnt == 3) {
                int party = Utility.RandomIndexWithProbability(new int[] { 20, 30, 50 });
                HeroParty hp = new HeroParty();
                for (int i = 0; i < party; ++i) {
                    hp.PartyMembers.Add(_heros[listIndex++]);
                }
                _heroParties.Add(hp);
                cnt -= party;
            }
            // 4-3. 2인 일 경우     1인 30% 2인 70%
            else if (cnt == 2) {
                int party = Utility.RandomIndexWithProbability(new int[] { 30, 70 });
                HeroParty hp = new HeroParty();
                for (int i = 0; i < party; ++i) {
                    hp.PartyMembers.Add(_heros[listIndex++]);
                }
                _heroParties.Add(hp);
                cnt -= party;
            }
            // 4-4. 1인 일 경우     1인 100%
            else if (cnt == 1) {
                HeroParty hp = new HeroParty();
                hp.PartyMembers.Add(_heros[listIndex++]);
                _heroParties.Add(hp);
                cnt -= 1;
            }
        }
    }

    private void SetLeader() {
        for(int i = 0; i < _heroParties.Count; ++i) {
            try
            {
                // 파티원이 혼자일 경우 자신이 리더
                if (_heroParties[i].PartyMembers.Count == 1)
                {
                    _heroParties[i].Leader = _heroParties[i].PartyMembers[0];
                    continue;
                }
                // 파티원 랭크 중 가장 높고 빨리 합류한 사람이 리더
                _heroParties[i].Leader = _heroParties[i].PartyMembers[0];
                for (int j = 0; j < _heroParties[i].PartyMembers.Count; ++j)
                {
                    GameObject next = _heroParties[i].PartyMembers[j];
                    if ((int)_heroParties[i].Leader.GetComponentInChildren<Hero>()._Rank < (int)next.GetComponentInChildren<Hero>()._Rank)
                        _heroParties[i].Leader = next;
                }
            }
            catch { Debug.LogError(i + " 번째 / " + _heroParties[i].Leader); }
        }
    }

    private void SetPartyName() {
    }

    private void CreateHeroSpawnDescription() {
        _heroSpawnDescriptions = new HeroSpawnDescription[] {
            new HeroSpawnDescription(new HeroRank[] {HeroRank.VILLIGER, HeroRank.ADVENTURER }, 45),
            new HeroSpawnDescription(new HeroRank[] {HeroRank.VILLIGER }, 6)
        };
    }
}

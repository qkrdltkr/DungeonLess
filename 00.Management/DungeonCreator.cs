using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ElementalAttributes { NONPASS = -1, NATURE, WATER, DARKNESS, FLAME, LIGHT}

[System.Serializable]
public class MapIndex {
    public MapIndex(int x, int y) { this.x = x; this.y = y; }
    public MapIndex() { }

    public int x;
    public int y;

    public void MoveLeft()  { y--; }
    public void MoveRight() { y++; }
    public void MoveUp()    { x--; }
    public void MoveDown()  { x++; }

    public void Set(int x, int y) { this.x = x; this.y = y; }
    public void Set(MapIndex index) { this.x = index.x; this.y = index.y; }

}

public struct Point {
    public Point(int x, int y) { this.x = x; this.y = y; }
    public int x;
    public int y;
}

public class DungeonCreator : MonoBehaviour {
    private struct RoomDireation {
        public const int roomCount = 9;
        public const int kBR       = 0;
        public const int kLRB      = 1;
        public const int kLB       = 2;
        public const int kTRB      = 3;
        public const int kLTRB     = 4;
        public const int kLTB      = 5;
        public const int kTR       = 6;
        public const int kLTR      = 7;
        public const int kLT       = 8;
    }
    [System.Serializable]
    public struct Size {
        public int width;
        public int height;
    }

    [System.Serializable]
    public struct DungeonPack {
        public ElementalAttributes Atribute;
        public Transform[] BR;
        public Transform[] LRB;
        public Transform[] LB;
        public Transform[] TRB;
        public Transform[] LTRB;
        public Transform[] LTB;
        public Transform[] TR;
        public Transform[] LTR;
        public Transform[] LT;
    }
    [Header("- DungeonType")]
    public Size _MapSize;
    public Transform[,] DungeonMap { get; private set; }

    public DungeonPack[] _DungeonPacks;
    private DungeonPack _dungeonPack;

    private Transform[][] _rooms;        // 결과
    private List<Point> _eventIndices;   // 이벤트 방 배정 결과

    [Space(10)] [Header("- Entrance")]
    public GameObject _EntrancePrefab;
    private GameObject _entrance;

    public ElementalAttributes CreateRooms() {
        _dungeonPack = _DungeonPacks[Random.Range(0, _DungeonPacks.Length)];
        
        RegisterRoom();
        SpawnRoom();
        LinkPath();
        SetEntrance();
   
        return _dungeonPack.Atribute;
    }
    
    #region Rooms
    private void RegisterRoom() {
        // 방 종류 할당
        _rooms = new Transform[RoomDireation.roomCount][];
        Transform[][] roomKinds = SortRoomKinds();
        // 2차원 동적 할당
        for (int i = 0; i < roomKinds.Length; ++i) {
            _rooms[i] = new Transform[roomKinds[i].Length];
        }
        // 방 등록
        for (int i = 0; i < _rooms.Length; ++i) {
            for (int j = 0; j < _rooms[i].Length; ++j) {
                _rooms[i][j] = roomKinds[i][j];
            }
        }
    }

    private Transform[][] SortRoomKinds() {
        Transform[][] roomKinds = new Transform[9][];        
        
        roomKinds[RoomDireation.kBR]   = _dungeonPack.BR;
        roomKinds[RoomDireation.kLRB]  = _dungeonPack.LRB;
        roomKinds[RoomDireation.kLB]   = _dungeonPack.LB;
        roomKinds[RoomDireation.kTRB]  = _dungeonPack.TRB;
        roomKinds[RoomDireation.kLTRB] = _dungeonPack.LTRB;
        roomKinds[RoomDireation.kLTB]  = _dungeonPack.LTB;
        roomKinds[RoomDireation.kTR]   = _dungeonPack.TR;
        roomKinds[RoomDireation.kLTR]  = _dungeonPack.LTR;
        roomKinds[RoomDireation.kLT]   = _dungeonPack.LT;


        return roomKinds;
    }

    private void SpawnRoom() {
        // 0. 이벤트 방 갯수 정하기
        int itemRoomCount = Mathf.RoundToInt(_MapSize.width * _MapSize.height / 10.0f);
        int bossRommCount = Mathf.RoundToInt(itemRoomCount / 2.0f);
        _eventIndices = new List<Point>();

        System.Func<Point, bool> check = (index) => {
            for (int i = 0; i < _eventIndices.Count; ++i)
                if (_eventIndices[i].x == index.x && _eventIndices[i].y == index.y)
                    return true;
            return false;
        };

        // 0-1. 아이템 방 갯수
        MapIndex[] itemRoomIndices = new MapIndex[itemRoomCount];
        for (int i = 0; i < itemRoomIndices.Length; ++i)
            itemRoomIndices[i] = new MapIndex(-1, -1);
        for(int i = 0; i < itemRoomIndices.Length; ++i) {
        ITEM_REROLL:
            MapIndex index = new MapIndex();
            index.x = Random.Range(0, _MapSize.height);
            index.y = Random.Range(0, _MapSize.width);
            for (int j = i; j >= 0; --j) {
                if (index.x == itemRoomIndices[j].x && index.y == itemRoomIndices[j].y) goto ITEM_REROLL;
                
            }
            if(check(new Point(index.x, index.y))) goto ITEM_REROLL;
            else {
                itemRoomIndices[i] = index;
                _eventIndices.Add(new Point(index.x, index.y));
            }
        }
        // 0-2. 보스 방 갯수
        MapIndex[] bossRoomIndices = new MapIndex[bossRommCount];
        for (int i = 0; i < bossRoomIndices.Length; ++i)
            bossRoomIndices[i] = new MapIndex(-1, -1);
        for (int i = 0; i < bossRoomIndices.Length; ++i) {
        BOSS_REROLL:
            MapIndex index = new MapIndex();
            index.x = Random.Range(0, _MapSize.height);
            index.y = Random.Range(0, _MapSize.width);
            for (int j = i; j >= 0; --j) {
                if (index.x == bossRoomIndices[j].x && index.y == bossRoomIndices[j].y) goto BOSS_REROLL;
            }
            if (check(new Point(index.x, index.y))) goto BOSS_REROLL;
             else {
                bossRoomIndices[i] = index;
                _eventIndices.Add(new Point(index.x, index.y));
            }
        }
        // 1. 맵 생성
        DungeonMap = new Transform[_MapSize.height, _MapSize.width];
        // 2. 방들의 부모가 될 상위 객체 탐색 및 생성
        GameObject roomParent = GameObject.Find("Dungeon");
        if (!roomParent) roomParent = new GameObject("Dungeon");
        // 3. 방 배치
        for (int n = 0; n < _MapSize.height; ++n) {
            for (int m = 0; m < _MapSize.width; ++m) {
                int kind = CheckRoom(n, m);

                int room = -1;
                // 일반방 할당
                while (true) {
                    room = Random.Range(0, _rooms[kind].Length);
                    if (!(_rooms[kind][room].GetComponent<Room>()._RoomKind == RoomKind.NORMAL))
                        continue;
                    break;
                }
                // 아이템방 할당
                for (int i = 0; i < itemRoomIndices.Length; ++i) {
                    if(itemRoomIndices[i].x == n && itemRoomIndices[i].y == m)
                        for (int j = 0; j < _rooms[kind].Length; ++j) { // 아이템 방 찾기 (랜덤 아님)
                            if (_rooms[kind][j].GetComponent<Room>()._RoomKind == RoomKind.ITEM) {
                                room = j;
                                break;
                            }
                    }
                }
                // 보스방 할당
                for (int i = 0; i < bossRoomIndices.Length; ++i){
                    if (bossRoomIndices[i].x == n && bossRoomIndices[i].y == m)
                        for (int j = 0; j < _rooms[kind].Length; ++j) {
                            if (_rooms[kind][j].GetComponent<Room>()._RoomKind == RoomKind.BOSS) {
                                room = j;
                                break;
                            }
                        }
                }

                DungeonMap[n,m] = Instantiate(_rooms[kind][room], 
                    new Vector3(m * 150.0f, n * -150.0f, ObjectSortLevel.kTile), Quaternion.identity, roomParent.transform); // Position, Quaternion for Debug
            }
        }
        // 4. 방 번호 먹이기
        for (int n = 0; n < _MapSize.height; ++n) {
            for (int m = 0; m < _MapSize.width; ++m) {
                DungeonMap[n, m].GetComponent<Room>().Index = new MapIndex(n, m);
            }
        }

        // 5. 바닥 콜라이더 Inacitve
        GameObject[] floorColls = GameObject.FindGameObjectsWithTag("Floor");
        for (int i = 0; i < floorColls.Length; ++i)
            floorColls[i].GetComponent<Collider>().enabled = false;
    }

    private void LinkPath() {
        // 생성된 방들 연결하기
        for (int n = 0; n < DungeonMap.GetLength(0); ++n) {
            for (int m = 0; m < DungeonMap.GetLength(1); ++m) {                
                DoorPath[] paths = DungeonMap[n, m].GetComponentsInChildren<DoorPath>();
                for (int k = 0; k < paths.Length; ++k) {
                    switch (paths[k]._PathType) {
                        case PathType.LEFT  : Linking(paths[k], n, m-1);  break;
                        case PathType.TOP   : Linking(paths[k], n-1, m);  break;
                        case PathType.RIGHT : Linking(paths[k], n, m+1);  break;
                        case PathType.BOTTOM: Linking(paths[k], n+1, m);  break;
                    }
                }                
            }
        }
    }

    private void Linking(DoorPath path, int n, int m) {
        DoorPath[] nextPaths = DungeonMap[n, m].GetComponentsInChildren<DoorPath>();

        for (int i = 0; i < nextPaths.Length; ++i) {
            if        (path._PathType == PathType.LEFT && nextPaths[i]._PathType == PathType.RIGHT) {
                path.SetNextPath(nextPaths[i].transform);
            } else if (path._PathType == PathType.TOP && nextPaths[i]._PathType == PathType.BOTTOM) {
                path.SetNextPath(nextPaths[i].transform);
            } else if (path._PathType == PathType.RIGHT && nextPaths[i]._PathType == PathType.LEFT) {
                path.SetNextPath(nextPaths[i].transform);
            } else if (path._PathType == PathType.BOTTOM && nextPaths[i]._PathType == PathType.TOP) {
                path.SetNextPath(nextPaths[i].transform);
            }
        }
    }

    private void SetEntrance() {
        // 입구 후보 탐색
        GameObject[] entrances = GameObject.FindGameObjectsWithTag("Entrance");
        // 입구 선정
        while (true) {
            _entrance = entrances[Random.Range(0, entrances.Length)];
            if (_entrance.transform.parent.parent.GetComponent<Room>()._RoomKind == RoomKind.NORMAL)
                break;
        }
        // 입구 생성
        _entrance = Instantiate(_EntrancePrefab, _entrance.transform.position, Quaternion.identity, _entrance.transform.parent);
        _entrance.name = "Door";
        // 후보 파괴
        for (int i = 0; i < entrances.Length; ++i) Destroy(entrances[i]);
    }

    private int CheckRoom(int n, int m) {
        /*
         * 0, 0 → DR   /   0, m → LB   /   n, 0 → TR   /   n, m → LT
         * 0,        1 ~ m-1         → LRB
         * 1 ~ n-1,  0               → TRB
         * 1 ~ n-1,  m               → LTB
         * n,        1 ~ m-1         → LTR
         * Etc                       → LTRB
        */
        if      (n == 0 && m == 0)                                        return RoomDireation.kBR;
        else if (n == 0 && m == _MapSize.width-1)                         return RoomDireation.kLB;
        else if (n == _MapSize.height-1 && m == 0)                        return RoomDireation.kTR;
        else if (n == _MapSize.height-1 && m == _MapSize.width-1)         return RoomDireation.kLT;
        else if (n == 0 && m > 0 && m < _MapSize.width-1)                 return RoomDireation.kLRB;
        else if (n > 0 && n < _MapSize.height-1 && m == 0)                return RoomDireation.kTRB;
        else if (n > 0 && n < _MapSize.height-1 && m == _MapSize.width-1) return RoomDireation.kLTB;
        else if (n == _MapSize.height-1 && m > 0 && m < _MapSize.width-1) return RoomDireation.kLTR;
        else                                                              return RoomDireation.kLTRB;
    }
    #endregion

    public GameObject SpawnPlayer(GameObject playerPrefab) {
        // 플레이어 생성
        Transform startRoom = _entrance.transform.parent.parent;
        MapIndex mapIndex = new MapIndex(0,0);
        // 내 위치 찾아서 저장, 플레이어에 등록
        for (int i = 0; i < DungeonMap.GetLength(0); i++) {
            for (int j = 0; j < DungeonMap.GetLength(1); j++) {
                if (startRoom.Equals(DungeonMap[i, j])) {
                    mapIndex.x = i; mapIndex.y = j;
                    break;
                }
            }
        }
        Transform playerSpawnPos = startRoom.Find("PlayerSpawnPos");
        Vector3 pos = new Vector3(playerSpawnPos.position.x, playerSpawnPos.transform.position.y, ObjectSortLevel.kCharacter);
        GameObject go = Instantiate(playerPrefab, pos, Quaternion.identity);
        go = go.transform.Find("HitBox").gameObject;
        go.transform.GetChild(0).parent = null;
        go.GetComponent<PlayerController>().Index = mapIndex;

        // 시작 방 세팅
        Room room = DungeonMap[mapIndex.x, mapIndex.y].GetComponent<Room>();
        // 몬스터 삭제
        room.EmptyPaths();
        room._MonsterNum = 0;
        // 함정 삭제
        TrapSpawner[] traps = room.transform.GetComponentsInChildren<TrapSpawner>();
        for (int i = 0; i < traps.Length; ++i)  traps[i]._SpawnKind = null;

        return go;
    }
}

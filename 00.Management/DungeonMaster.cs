using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode {
    NONPASS = -1,
    ROGUE_LIKE, // 로그라이크
    SHOP,       // 상점
    PLACEMENT,  // 배치
    DEFENSE     // 수비
}

// 운영 데이터
public static class OperationData { 
    public static bool IsConsoleOn;
    public static bool IsAttackBlock;
    public static Transform Player;  // 플레이어
    public static Transform Weapons; // 무기
    public static Transform TargetMonster;
    public static Transform TargetTrap;
    public static Transform[,] MapData;
}

public class DungeonMaster : MonoBehaviour {
    public static int Stage = 0;
    // Manager
    private DungeonCreator _dungeonCreator;
    private MainCamController _cameraController;
    private MonsterManager _monsterManager;
    private TrapManager _trapManager;
    private UIManager _uiManager;
    private MapManager _mapManager;
    private ItemManager _itemManager;
    private HeroManager _heroManager;
    // Game
    public static GameMode Mode;
    private ElementalAttributes _dungeonType;
    private Collider[] _placementAreas;
    // Player
    public GameObject _PlayerPrefab;
    private GameObject _player;

    void Awake() {
        Initialize();
        CreateDungeon();
        CreateTrap();
        CreateMonster();
        InitMap();
        CreateUI();
        InitItem();
        InitHero();

    }

    private void Initialize() {
        _dungeonCreator = GameObject.Find("Managers/DungeonCreator").GetComponent<DungeonCreator>();
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamController>();
        _monsterManager = GameObject.Find("Managers/MonsterManager").GetComponent<MonsterManager>();
        _trapManager = GameObject.Find("Managers/TrapManager").GetComponent<TrapManager>();
        _uiManager = GameObject.Find("Managers/UIManager").GetComponent<UIManager>();
        _mapManager = GameObject.Find("Managers/MapManager").GetComponent<MapManager>();
        _itemManager = GameObject.Find("Managers/ItemManager").GetComponent<ItemManager>();
        _heroManager = GameObject.Find("Managers/HeroManager").GetComponent<HeroManager>();

        Mode = GameMode.ROGUE_LIKE;
    }

    private void CreateDungeon() {
        // 아이템 배열 초기화
        ItemSpawner.Instance.Init();
        // 방 생성
        _dungeonType = _dungeonCreator.CreateRooms();
        // 플레이어 스폰 및 카메라 세팅
        _player = _dungeonCreator.SpawnPlayer(_PlayerPrefab);
        _cameraController.Init(_player);

        // 던전 전역 등록
        OperationData.MapData = _dungeonCreator.DungeonMap;
        // 플레이어 전역 등록
        OperationData.Player = _player.transform.parent;
        // 무기 전역 등록
        OperationData.Weapons = GameObject.Find("Weapons").transform;
    }
    
    private void CreateMonster() {
        _monsterManager.DungeonType = _dungeonType;
        _monsterManager.AllMonsterSpawn(_player.transform);
    }

    private void CreateTrap() {
        _trapManager.Initialize();
        _trapManager.AllTrapSpawn();
    }

    private void CreateUI() {
        _uiManager.Init(_player.GetComponent<Transform>());
        _uiManager.SetStatsUI();
        _uiManager.InitMaps(_dungeonCreator.DungeonMap);
    }

    private void InitMap() {
        MapIndex index = _player.GetComponent<PlayerController>().Index;

        _mapManager.DungeonMap = _dungeonCreator.DungeonMap;
        _mapManager.PlayerIndex = index;
        _mapManager.ClearMonster();

        UIManager.AddIndex(new Point(index.x, index.y));
    }

    private void InitItem(){
        _itemManager.Init(_player.GetComponent<PlayerController>());
    }

    private void InitHero() {
        _heroManager.Init();
    }

    public void ChangeMode(GameMode mode) {
        if (Mode == mode) return;
        Mode = mode;
        switch (mode)  {
            case GameMode.ROGUE_LIKE:          break;
            case GameMode.SHOP:                break;
            case GameMode.PLACEMENT: {
                    GameObject[] floorColls = GameObject.FindGameObjectsWithTag("Floor");
                    for (int i = 0; i < floorColls.Length; ++i)
                        floorColls[i].GetComponent<Collider>().enabled = true;
                    break;
                }
            case GameMode.DEFENSE: {
                    GameObject[] floorColls = GameObject.FindGameObjectsWithTag("Floor");
                    for (int i = 0; i < floorColls.Length; ++i)
                        floorColls[i].GetComponent<Collider>().enabled = false;
                    _heroManager.Pop();
                    break;
                }
        }

        _uiManager.SetUI(mode);
    }

    public IEnumerator EndGame() {
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
        StartCoroutine(_uiManager.Fade(true));
        _player.SetActive(false);
    }

    public void NextLevel() {
        // 아이템 정리
        // 1. 방 전부 Enable (오브젝트 활성화해야됨)
        // 2. 방에 깔려 있는 아이템 회수해서 push
    }
}

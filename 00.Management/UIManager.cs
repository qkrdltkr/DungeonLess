using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//==========================================================
// 분할 안된 막코드
// 캔버스 별로 나중에 분할해야됨
// 로그라이크, 샵, 배치 등등
//==========================================================
public class UIManager : IUpdateableObject {
    private Transform[,] _dungeonMap;
    // UI Canvas
    public GameObject _BasicCanvas;
    public GameObject _WorldCanvas;
    // 부속 UI
    private Transform _shop;
    private Transform _placement;
    // 이동했던 인덱스
    private static List<Point> s_indexList = new List<Point>();
    public static List<Point>  IndexList {
        get { return s_indexList; }
        private set { s_indexList = value; }
    }
    //========================================================
    // 컷씬
    private Transform _cutScene;
    // 로그라이크 맵
    public MaxMapCamController _MaxMapCamController;
    public static bool IsMaxMapOn = false;
    public static bool IsInventory = false;
    private bool _isBattle;

    // 맵
    public GameObject _PlacementMap;
    public GameObject _DefenseMap;

    public Transform _MiniMap;
    public Transform _MaxMap;
    public GameObject _MagnifyRoomPrefab;
    //=========================================================
    // Player
    private PlayerController _playerController;
    public GameObject _NecronomiconBook;
    public GameObject _SpellBook;
    // 돈
    private Text _money;
    private Text _magicStone;
    private Text _repair;
    private Text _population;

    public void Init(Transform playerTr) {
        _MaxMap.localScale = Vector3.zero;
        _playerController = playerTr.GetComponent<PlayerController>();
        _WorldCanvas.GetComponent<StatusIcon>().Init(playerTr);
    }

    protected override void Initialize() {
        // 기타 UI 초기화
        _MaxMap.GetComponent<MagnifyMap>().Synchronization();
        _NecronomiconBook.SetActive(false);
        _SpellBook.SetActive(false);
        // 부속 UI 초기화
        _shop      = _BasicCanvas.transform.Find("Shop");
        _placement = _BasicCanvas.transform.Find("Placement");
        _cutScene = _BasicCanvas.transform.Find("CutScene");

        // Status UI 초기화
        _money = _BasicCanvas.transform.Find("Common/Status/Gold/Text").GetComponent<Text>();
        _magicStone = _BasicCanvas.transform.Find("Common/Status/MagicStone/Text").GetComponent<Text>();
        _repair = _BasicCanvas.transform.Find("Placement/Info/Repair/Text").GetComponent<Text>();
        _population = _BasicCanvas.transform.Find("Placement/Info/Population/Text").GetComponent<Text>();
    }

    public override void OnUpdate(float dt) {
        InputKey();
    }
    private void InputKey() {
        if (OperationData.IsConsoleOn) return;
        RogueLikeControll();
    }

    private void RogueLikeControll() {
        if (_isBattle) return;

        if (!IsInventory) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                IsMaxMapOn = true;

                _MaxMapCamController.EnableCam();
                _MaxMap.localScale = Vector3.one;
                _MiniMap.localScale = Vector3.zero;
            } else if (Input.GetKeyUp(KeyCode.Tab)) {
                IsMaxMapOn = false;

                _MaxMapCamController.DisableCam();
                _MaxMap.localScale = Vector3.zero;
                _MiniMap.localScale = Vector3.one;
            }
        }

        if (!IsMaxMapOn) {
            if (Input.GetKeyDown(KeyCode.I)) {
                IsInventory = !IsInventory;
                if (_playerController._IsSpellbook) {
                    _SpellBook.SetActive(!_SpellBook.activeInHierarchy);
                    _NecronomiconBook.SetActive(false);
                } else {
                    _NecronomiconBook.SetActive(!_NecronomiconBook.activeInHierarchy);
                    _SpellBook.SetActive(false);
                }
            } else if (Input.GetKeyDown(KeyCode.Escape))  {
                IsInventory = false;
                _SpellBook.SetActive(false);
                _NecronomiconBook.SetActive(false);
                _SpellBook.SetActive(false);
                _NecronomiconBook.SetActive(false);
            }
        }
    }

    public void SetBattle(bool isOn) {
        if (isOn) {
            _isBattle = isOn;

            _MaxMapCamController.DisableCam();
            _MaxMap.localScale = Vector3.zero;
            _MiniMap.localScale = Vector3.one;

            IsMaxMapOn = false;
            IsInventory = false;
            _SpellBook.SetActive(false);
            _NecronomiconBook.SetActive(false);
            _SpellBook.SetActive(false);
            _NecronomiconBook.SetActive(false);

        } else _isBattle = false;
    }

    public void InitMaps(Transform[,] dungeonMap) {
        _dungeonMap = dungeonMap;
        // 200 / 200 기준
        // 시작 점 -200 200
        Vector2 pos = new Vector2(-200.0f, 200.0f);
        // 간격은 X : 150 Y -150
        GameObject go = null;
        GameObject[,] rooms = new GameObject[dungeonMap.GetLength(0), dungeonMap.GetLength(1)];
        for (int i = 0; i < dungeonMap.GetLength(0); ++i) {
            for (int j = 0; j < dungeonMap.GetLength(1); ++j) {
                go = Instantiate(_MagnifyRoomPrefab, _MaxMap);

                RectTransform rectTransform = go.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = pos;
                rectTransform.localScale = Vector3.one;
                pos = new Vector2(pos.x + 150.0f, pos.y);

                rooms[i, j] = go;
            }
            pos = new Vector2(pos.x = -200.0f, pos.y -= 150.0f);
        }

        _MaxMap.GetComponent<MagnifyMap>().Init(rooms);

        // 0. PlacementMap에 dungeonMap 전달하기
        _PlacementMap.transform.Find("Map/Rooms").GetComponent<PlacementMap>().Init(dungeonMap);
        _BasicCanvas.transform.Find("Placement").GetComponentInChildren<PlacementCursor>().Map = dungeonMap;
        // 1. DefenseMap에 전달
        _DefenseMap.transform.Find("Map/Rooms").GetComponent<DefenseMap>().Init(dungeonMap);
    }

    public void SetStatsUI() {
        Character.Stats stats = _playerController.GetStats();
        _BasicCanvas.transform.Find("Common/Status/MaxHeatPoint/HeatPointBar").GetComponent<HeatPointBar>().Init(stats);
        _BasicCanvas.transform.Find("Common/Status/MaxManaPoint/ManaPointBar").GetComponent<ManaPointBar>().Init(stats);
    }
    public void UpdateMoney(int money) { _money.text = "x" + money;}
    public void UpdateMagicStone(int stone) { _magicStone.text = "x" + stone;}
    public void UpdateRepair(int repair) { _repair.text = "x" + repair; }
    public void UpdatePopulation(int current, int maximum) { _population.text = current + "/" + maximum; }

    public IEnumerator Fade(bool isIn){
        RawImage image = _BasicCanvas.transform.Find("Fade").GetComponent<RawImage>();
        if (isIn) {
            // FadeIn
            for (int i = 1; i <= 10; ++i) {
                image.color = new Color(0.0f, 0.0f, 0.0f, i * 0.1f);
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }
        } else {
            for (int i = 10; i >= 0; --i) {
                image.color = new Color(0.0f, 0.0f, 0.0f, i * 0.1f);
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }
        }
    }

    public void SetUI(GameMode mode) {
        // 세팅
        switch (mode) {
            case GameMode.ROGUE_LIKE:                                     break;
            case GameMode.SHOP:         StartCoroutine(SetShopUI());      break;
            case GameMode.PLACEMENT:    StartCoroutine(SetPlacementUI()); break;                
            case GameMode.DEFENSE:      StartCoroutine(SetDefenseUI());   break;
                
        }
    }

    private IEnumerator SetShopUI() {
        // 인벤토리 닫기
        IsInventory = false;
        _SpellBook.SetActive(false);
        _NecronomiconBook.SetActive(false);
        // 마우스 변경
        MouseCursor.Instance.Set(CursorIcon.kBasic);

        // FadeIn
        StartCoroutine(Fade(true));
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
        // CutScene
        float cutDelay = PlayCutScene(GameMode.SHOP);
        // 미리 배치해야 될 것들
        yield return YieldInstructionCache.WaitForSeconds(cutDelay + 0.5f);
        _cutScene.gameObject.SetActive(false);
        OperationData.Player.GetComponentInChildren<PlayerController>().Placement();

        OperationData.Weapons.gameObject.SetActive(false);
        // 로그라이크 UI 끄기
        _BasicCanvas.transform.Find("RogueLike").gameObject.SetActive(false);
        // 상점 UI 켜기
        _shop.gameObject.SetActive(true);
        // 랜덤 상인 활성화
        int r = Random.Range(0, _shop.childCount);
        _shop.GetChild(r).gameObject.SetActive(true);
        _shop.GetChild(r).GetComponent<IShop>().OpenShop();

        // FadeOut
        StartCoroutine(Fade(false));
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
    }

    private IEnumerator SetPlacementUI() {
        // 인벤토리 닫기
        IsInventory = false;
        _SpellBook.SetActive(false);
        _NecronomiconBook.SetActive(false);

        // FadeIn
        StartCoroutine(Fade(true));
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
        // CutScene
        float cutDelay = PlayCutScene(GameMode.PLACEMENT);
        // 미리 배치해야 될 것들
        yield return YieldInstructionCache.WaitForSeconds(cutDelay + 0.5f);
        _cutScene.gameObject.SetActive(false);

        _shop.gameObject.SetActive(false);

        Room room = _dungeonMap[_playerController.Index.x, _playerController.Index.y].GetComponent<Room>();
        room._MonsterNum = room.MaximumMonsterNum;
        UpdatePopulation(room._MonsterNum, room.MaximumMonsterNum);

        // 배치 코딩 할 곳
        _BasicCanvas.transform.Find("Common").gameObject.SetActive(false);
        _PlacementMap.SetActive(true);
        _placement.gameObject.SetActive(true);
        _placement.Find("Timer").GetComponent<PlacementTimer>().StartCoroutine("StartTimer");

        // FadeOut
        StartCoroutine(Fade(false));
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
    }

    private IEnumerator SetDefenseUI() {
        // Placement UI 닫기
        PlacementTrapToolTip.Instance.OffToolTip();
        PlacementMonsterToolTip.Instance.OffToolTip();
        _placement.Find("List").GetComponent<PlacementCursor>().ClearTarget();
        _PlacementMap.SetActive(false);

        // FadeIn
        StartCoroutine(Fade(true));
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
        // CutScene
        float cutDelay = PlayCutScene(GameMode.DEFENSE);
        // 미리 배치해야 될 것들
        yield return YieldInstructionCache.WaitForSeconds(cutDelay + 0.5f);
        _cutScene.gameObject.SetActive(false);
        _PlacementMap.GetComponent<PlacementUI>().Reset();
        _placement.gameObject.SetActive(false);
        // FadeOut
        StartCoroutine(Fade(false));
        // 입구 비추기
        MainCamController mcc = Camera.main.GetComponent<MainCamController>();
        Vector3 entrancePos = GameObject.FindGameObjectWithTag("Entrance").transform.position;
        mcc.transform.position = new Vector3(entrancePos.x, entrancePos.y, -100.0f);
        //=====================================
        // 용사 쳐들어오는 거 보여주기
        yield return YieldInstructionCache.WaitForSeconds(5.0f);
        //=====================================
        // 카메라 돌리기
        mcc.transform.position = OperationData.MapData[mcc.Index.x, mcc.Index.y].Find("Monsters").position;

        StartCoroutine(Fade(true));
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
        // 배치 코딩 할 곳
        _DefenseMap.SetActive(true);
        StartCoroutine(Fade(false));
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
    }

    private float PlayCutScene(GameMode mode) {
        _cutScene.gameObject.SetActive(true);
        // 컷씬 재생
        CutScene cutScene = _cutScene.GetComponent<CutScene>();
        float dealy = cutScene.Init(mode);
        cutScene.StartCoroutine("Animate");
        // 컷씬 종료     
        return dealy;
    }

    public static void AddIndex(Point point)  {
        for (int i = 0; i < s_indexList.Count; ++i) {
            if (s_indexList[i].x == point.x && s_indexList[i].y == point.y)        
                return;            
        }           
        s_indexList.Add(point);
    }
    public static void ClearIndex() {
        s_indexList.Clear();
    }
}

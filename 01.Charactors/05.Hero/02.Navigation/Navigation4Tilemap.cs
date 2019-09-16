using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
public class NodeItem {
    public bool _isWall;
    // node position
    public Vector2 _pos;
    public int x, y;
    public int _costToStart;
    public int _costToEnd;

    public int CostTotal {
        get { return _costToStart + _costToEnd; }
    }

    // parent node
    public NodeItem _parent;

    public NodeItem(bool isWall, Vector2 pos, int x, int y) {
        this._isWall = isWall;
        this._pos = pos;
        this.x = x;
        this.y = y;
    }
}

public class Navigation4Tilemap : MonoBehaviour {
    //벽표시 On/Off
    public bool _ShowWallMark = false;
    //경로표시 On/Off
    public bool _ShowPath = true;
    //대각선 or 수직이동 방식

    private bool _startFinding;
    //맵의 시작지점, 끝지점
    public Vector2 TilemapStart { get; set; }
    public Vector2 TilemapEnd { get; set; }
         
    private Vector3 _destPos;
    private GameObject _wallMarkPrefab, _pathMarkPrefab;
    private GameObject _wallMarks, _pathMarks;
    //시작지점의 축
    private GameObject _axis;
    //벽 레이어
    private LayerMask _wallLayer;

    private List<NodeItem> _pathNodes;
    //검사할 영역
    private float _radius = 0.3f;
    
    
    public NodeItem[,] Map { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    private Transform _hero;

    private List<GameObject> _pathObjs = new List<GameObject>();

   
    private void Awake() {
      _ShowWallMark = false;
        //마크 생성
        CreateMarks();
        //레이어 설정
        SetLayerInit();
        //노드 생성
        InitNavigationMap();
    }

    private void CreateMarks() {
        //wallMark 생성 및 부모지정
        _wallMarks = new GameObject("WallMarks");
        _wallMarks.transform.SetParent(this.transform);
        //pathMark 생성 및 부모지정
        _pathMarks = new GameObject("PathMarks");
        _pathMarks.transform.SetParent(this.transform);
        //축 가져오ㅊ기
        _axis = this.gameObject.transform.parent.Find("Axis").gameObject;
        //프리팹 가져오기
        _wallMarkPrefab = GameObject.Find("MapManager").GetComponent<MapManager>()._ObstaclePrefab;
        _pathMarkPrefab = GameObject.Find("MapManager").GetComponent<MapManager>()._PathNodePrefab;

    }

    protected void SetLayerInit() {
        //TileWall이라는 레이어만 검사하겠다.
        _wallLayer = (-1) - (1 << LayerMask.NameToLayer("TileWall"));
        _wallLayer = ~_wallLayer;
    }
    public void InitNavigationMap() {
        //마크 삭제
        DestroyMarks();
        //타일 사이즈 가져오기
        GetTileMapEndSize();

        Width = Mathf.RoundToInt(TilemapEnd.x - TilemapStart.x + 1); //가로
        Height = Mathf.RoundToInt(TilemapEnd.y - TilemapStart.y + 1); //세로
        Map = new NodeItem[Width, Height];

        // Wall 노드 생성
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++)  {
                Vector2 pos = new Vector2(TilemapStart.x + x, TilemapStart.y + y);
                // 영역의 Layer가 WallLayer인지 아닌지 검사
                bool isWall = Physics2D.OverlapCircle(pos + new Vector2(0.5f, 0.5f), _radius, _wallLayer);
                // 맵 생성
                Map[x, y] = new NodeItem(isWall, pos, x, y);
                // Wall 노드 생성
                if (isWall && _ShowWallMark && _wallMarkPrefab) {
                    GameObject obj = GameObject.Instantiate(_wallMarkPrefab, new Vector3(
                        pos.x + 0.5f, pos.y + 0.5f, 0), Quaternion.identity) as GameObject;
                    obj.transform.SetParent(_wallMarks.transform);
                }
            }
        }
    }

    private void DestroyMarks() {
        for (int i = 0; i < _wallMarks.transform.childCount; i++) {
            Destroy(_wallMarks.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < _pathMarks.transform.childCount; i++) {
            _pathMarks.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void GetTileMapEndSize() {
        Tilemap tilemap = GetComponent<Tilemap>();
        int tilemapEndX = 0,tilemapEndY = 0;
        int tilemapStartX = 0, tilemapStartY = 0;
        BoundsInt bounds = tilemap.cellBounds;
        //전체타일 가져오기
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        int xPos = 0, yPos = 0;
        for (xPos = 0; xPos < bounds.size.x; xPos++)  {
            for (yPos = 0; yPos < bounds.size.y; yPos++)  {
                TileBase tile = allTiles[xPos + yPos * bounds.size.x];
                if (tile != null) {
                    //x,y의 최대값(end) 구하기
                    if(tilemapEndX < xPos) tilemapEndX = xPos;
                    if (tilemapEndY < yPos) tilemapEndY = yPos;
                    //x,y의 최솟값(start) 구하기
                    if (tilemapStartX == 0) tilemapStartX = xPos;
                    if (tilemapStartX > xPos) tilemapStartX = xPos;
                    if (tilemapStartY == 0) tilemapStartY = yPos;
                    if (tilemapStartY > yPos) tilemapStartY = yPos;
                }
            }
        }
        //시작점 가져오기
        TilemapStart = new Vector2(_axis.transform.position.x, _axis.transform.position.y);


        // 끝지점 구하기
        tilemapEndX -= tilemapStartX;
        tilemapEndY -= tilemapStartY;

        // 끝지점 좌표로 변환
        TilemapEnd = new Vector2(tilemapEndX, tilemapEndY);
        TilemapEnd += new Vector2(TilemapStart.x +1, TilemapStart.y +1);
       
    }
}

public enum PathMode {
    diagonal,
    vertical
}

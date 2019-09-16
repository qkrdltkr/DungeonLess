using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTracePatternComponent : HeroPatternComponent {
    private Vector3 _destPos;
    private Transform _mapData;
    private Transform _randomMonster;

    private GameObject _wallMarkPrefab, _pathMarkPrefab;
    private GameObject _wallMarks, _pathMarks;
    private Navigation4Tilemap _navigation;


    private PathMode _PathMode = PathMode.diagonal;
    private List<NodeItem> _pathNodes;
    private List<GameObject> _pathObjs = new List<GameObject>();

    private float delaytime = 0;
    private float _heroMoveSpeed = 5f;
    private int _nodeIndex;
    private int _nodeMaxIndex;

    private bool _ShowPath = true;
    private bool _startFinding;
    private bool isCanHeroMove;

    public override void Run(float dt) {
        //길찾기
        if (_startFinding) Navigate();
        //Hero 이동
        if (isCanHeroMove) MoveHero(dt);
        //타겟이 없으면 추적 중지
        if (!_target) StopTrace();
        //추적 시작 or 길찾기 시작
        if (_isTracing) { Trace(dt, true); StopNavigate(); }
        else FindTarget(dt);
     
        //몬스터 리스트 갱신
        if (_monsters.Count > 0) UpdateList();

    }
    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _hero = origin;
        _hitBox = _hero.Find("HitBox");
        _heroStats = _hitBox.GetComponent<Hero>().GetStats();
        _condition = condition;
        Init();
    }
    public override void Init() {
        Transform monstersTr = GetMapData();
        for (int i = 0; i < monstersTr.childCount; ++i) _monsters.Add(monstersTr.GetChild(i));
        GetMarks();
    }
    public override void DoWork(Transform tr) {
        _isTracing = true; //추격가능
    }
    private void Navigate() {
        FindingPath(new Vector2(_hero.position.x, _hero.position.y), new Vector2(_destPos.x, _destPos.y));
        StopFinding();
        isCanHeroMove = true;
    }
    private void StopTrace() {
        _isTracing = false;
    }
    private void FindTarget(float dt) {
        //랜덤몬스터 길찾기
        delaytime += dt;
        if (delaytime > 0.5f) {
            if (!_randomMonster && _monsters.Count > 0) {
                _condition.SetCondition(Condition.kRun);
                _randomMonster = _monsters[Random.Range(0, _monsters.Count)];
            }
            if (_randomMonster) StartNavigate(_hero, _randomMonster);
            delaytime = 0;
        }
    }
    private void StartNavigate(Transform _hero, Transform _randomMonster) {
        _heroMoveSpeed = _heroStats.MoveSpeed;
        _destPos = _randomMonster.position;
        StartFinding();
    }
    private Transform GetMapData() {
        MapIndex myIndex = _hero.GetComponentInChildren<Character>().Index;
        _mapData = OperationData.MapData[myIndex.x, myIndex.y];
        Transform monstersTr = _mapData.Find("Monsters");
        _navigation = _mapData.GetComponentInChildren<Navigation4Tilemap>();
        return monstersTr;
    }
    private void GetMarks() {
        //마크 찾기
        _wallMarks = _navigation.transform.Find("WallMarks").gameObject;
        _pathMarks = _navigation.transform.Find("PathMarks").gameObject;
        _wallMarkPrefab = GameObject.Find("MapManager").GetComponent<MapManager>()._ObstaclePrefab;
        _pathMarkPrefab = GameObject.Find("MapManager").GetComponent<MapManager>()._PathNodePrefab;
    }
    public NodeItem GetNode(Vector2 position) {
        int x = Mathf.FloorToInt(position.x - _navigation.TilemapStart.x);
        int y = Mathf.FloorToInt(position.y - _navigation.TilemapStart.y);
        x = Mathf.Clamp(x, 0, _navigation.Width - 1);
        y = Mathf.Clamp(y, 0, _navigation.Height - 1);
        return _navigation.Map[x, y];
    }
    private void UpdateList() {
        //몬스터가 죽으면 리스트에서 제외
        for (int i = 0; i < _monsters.Count; ++i) {
            if (_monsters[i] == null) _monsters.RemoveAt(i);
        }
    }
    public List<NodeItem> GetNeighbourNodes(NodeItem curNode) {
        List<NodeItem> list = new List<NodeItem>();
        switch (_PathMode) {
            case PathMode.diagonal:
                for (int i = -1; i <= 1; i++) { //3
                    for (int j = -1; j <= 1; j++) { // 3
                        // 자신제외
                        if (i == 0 && j == 0)
                            continue;

                        int x = (curNode.x + i), y = (curNode.y + j);

                        // 범위안에 있는지 검사, 있으면 리스트에 추가
                        if (x < _navigation.Width && x >= 0 && y < _navigation.Height && y >= 0)
                            list.Add(_navigation.Map[x, y]);
                    }
                }
                break;
            case PathMode.vertical:
                if (curNode.x + 1 < _navigation.Width)
                    list.Add(_navigation.Map[curNode.x + 1, curNode.y]);
                if (curNode.x - 1 >= 0)
                    list.Add(_navigation.Map[curNode.x - 1, curNode.y]);
                if (curNode.y + 1 < _navigation.Height)
                    list.Add(_navigation.Map[curNode.x, curNode.y + 1]);
                if (curNode.y - 1 >= 0)
                    list.Add(_navigation.Map[curNode.x, curNode.y - 1]);
                break;
        }
        return list;
    }
    public void UpdatePath(List<NodeItem> lines) {
        int curListSize = _pathObjs.Count;
        if (_pathMarkPrefab && _ShowPath) {
            for (int i = 0, max = lines.Count; i < max; i++) {
                if (i < curListSize) { 
                    //이미 있다면 포지션 이동 (풀 개념)
                    _pathObjs[i].transform.position = lines[i]._pos + new Vector2(0.5f, 0.5f);
                    _pathObjs[i].SetActive(true);
                } else { //없으면 생성
                    GameObject obj = GameObject.Instantiate(_pathMarkPrefab, new Vector3(lines[i]._pos.x + 0.5f, lines[i]._pos.y + 0.5f, 0), Quaternion.identity) as GameObject;
                    obj.transform.SetParent(_pathMarks.transform);
                    _pathObjs.Add(obj);
                }
            }
            //해당되는 거 제외하고 다꺼버리기
            for (int i = lines.Count; i < curListSize; i++) {
                _pathObjs[i].SetActive(false);
            }
        }
        _nodeIndex = 0;
        _pathNodes = lines;
    }
    private void TurnOffNode(List<NodeItem> lines) {
        for (int i = 0; i < _pathObjs.Count; i++) {
            _pathObjs[i].SetActive(false);
        }
    }
    public void StopNavigate() {
        if (!isCanHeroMove) return;
        isCanHeroMove = false;
        StopFinding();
        if (_pathNodes != null) TurnOffNode(_pathNodes);
    }
    private void MoveHero(float dt) {
            bool isOver = false;
            _nodeMaxIndex = _pathNodes.Count;
            if (_nodeIndex >= _nodeMaxIndex) {
                StopNavigate();
                return;
            }
            if (!isOver) {
                Vector3 offSet = new Vector3(_pathNodes[_nodeIndex]._pos.x + 0.5f, _pathNodes[_nodeIndex]._pos.y + 0.5f, 0) - _hero.position;
                //이동
                _hero.position += offSet.normalized * _heroMoveSpeed * 3.0f * dt;
                _hero.GetComponentInChildren<Hero>().SetAnimationTarget(new Vector3(_pathNodes[_nodeIndex]._pos.x + 0.5f, _pathNodes[_nodeIndex]._pos.y + 0.5f, 0));
                //목표지점 도달 검사
                if (Vector2.Distance(_pathNodes[_nodeIndex]._pos + new Vector2(0.5f, 0.5f),
                    new Vector2(_hero.position.x, _hero.position.y)) < 0.1f) {
                    isOver = true;
                    _hero.position = new Vector3(_pathNodes[_nodeIndex]._pos.x + 0.5f, _pathNodes[_nodeIndex]._pos.y + 0.5f, 0);
                    _nodeIndex++;
                }
            }
    }
    private List<NodeItem> path = new List<NodeItem>();
    private void CreatePath(NodeItem startNode, NodeItem endNode) {
        path.Clear();
        if (endNode != null) {
            NodeItem temp = endNode;
         
            while (!temp.Equals(startNode)) {
                // 리스트에 endNdoe 부터 거꾸로 리스트에 넣는다
                path.Add(temp);
                temp = temp._parent;
            }
            path.Reverse(); //리스트 반전
        }
        //경로 갱신
        UpdatePath(path);
    }

    // A star 알고리즘
    private void FindingPath(Vector2 origin, Vector2 destination) {
        NodeItem startNode = GetNode(origin); //시작 노드 반환 
        NodeItem endNode = GetNode(destination); //끝 노드 반환 
        
        List<NodeItem> openSet = new List<NodeItem>();
        HashSet<NodeItem> closeSet = new HashSet<NodeItem>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            NodeItem curNode = openSet[0];
            for (int i = 0, max = openSet.Count; i < max; i++) {
                //최근노드보다 소요가 적을 경우 && 최종지점까지의 소요가 더 적을 경우
                if (openSet[i].CostTotal <= curNode.CostTotal &&
                    openSet[i]._costToEnd < curNode._costToEnd) {
                    curNode = openSet[i]; //최근 노드 저장
                }
            }

            openSet.Remove(curNode);
            closeSet.Add(curNode);

            // 목표지점에 도달했을 경우
            if (curNode==endNode) {
                //경로 생성
                CreatePath(startNode, endNode);
                return;
            }

            // 이웃한 노드중 가장 효율적인 노드 찾기
            foreach (var node in GetNeighbourNodes(curNode)) {
                //이웃한 노드를 돌면서 소요값 계산
                if (node._isWall || closeSet.Contains(node))
                    continue;
                //최근노드와 이웃한노드 거리 계산 및 소요 계산
                int newCost = curNode._costToStart + GetDistanceBetweenNodes(curNode, node);
                //소요가 가장 적은값 저장
                if (newCost < node._costToStart || !openSet.Contains(node)) {
                    node._costToStart = newCost;
                    //이웃한노드와 목표지점 거리계산
                    node._costToEnd = GetDistanceBetweenNodes(node, endNode);
                    //최근노드를 이웃한노드의 parent에 저장
                    node._parent = curNode;
                    if (!openSet.Contains(node)) {
                        openSet.Add(node);
                    }
                }
            }
        }
        //경로 생성
        CreatePath(startNode, null);
    }
    private int GetDistanceBetweenNodes(NodeItem curnode, NodeItem node) {
        // 최근노드와 이웃한 노드거리 계산
        int cntX = Mathf.Abs(curnode.x - node.x);
        int cntY = Mathf.Abs(curnode.y - node.y);
        if (cntX > cntY) {
            return 14 * cntY + 10 * (cntX - cntY);
        }
        else {
            return 14 * cntX + 10 * (cntY - cntX);
        }
    }

    public void StartFinding() {
        _startFinding = true;
    }

    public void StopFinding() {
        _startFinding = false;
    }
}

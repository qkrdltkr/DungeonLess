using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 1. 방 bool값, 클리어시 true
 * 2. 플레이어 주변 인덱스 방의 불 값 조사
 * 3. false일 경우 그 path조사해서 그 위에 물음표 만들기?
 */
public class MapManager : MonoBehaviour {
    public Transform[,] DungeonMap { get; set; }
    public MapIndex PlayerIndex { get; set; }
    private UIManager _uiManager;
    public GameObject _ObstaclePrefab;
    public GameObject _PathNodePrefab;

    void Start() {
        _uiManager = GameObject.Find("Managers/UIManager").GetComponent<UIManager>();
    }

    public void ClearMonster() {
        // 첫방 몬스터 삭제
        Transform monsters = DungeonMap[PlayerIndex.x, PlayerIndex.y].Find("Monsters");
        for (int i = 0; i < monsters.childCount; ++i)  {
            Destroy(monsters.GetChild(i).gameObject);
        }
        // 첫방을 제외한 모든 방 Disable
        for (int i = 0; i < DungeonMap.GetLength(0); ++i)  {
            for (int j = 0; j < DungeonMap.GetLength(1); ++j)  {
                if (i == PlayerIndex.x && j == PlayerIndex.y) continue;
                DungeonMap[i, j].gameObject.SetActive(false);
            }
        }
        DisableNowRoomQuestion();
    }

    private void DisableNowRoomQuestion() {
        int x, y;
        // 상
        x = PlayerIndex.x - 1; y = PlayerIndex.y;
        if (x > 0) DungeonMap[x, y].Find("Path/B").GetChild(0).gameObject.SetActive(false);
        // 하
        x = PlayerIndex.x + 1; y = PlayerIndex.y;
        if (x < DungeonMap.GetLength(0)) DungeonMap[x, y].Find("Path/T").GetChild(0).gameObject.SetActive(false);
        // 좌
        x = PlayerIndex.x; y = PlayerIndex.y - 1;
        if (y > 0) DungeonMap[x, y].Find("Path/R").GetChild(0).gameObject.SetActive(false);
        // 우
        x = PlayerIndex.x; y = PlayerIndex.y + 1;
        if (y < DungeonMap.GetLength(1)) DungeonMap[x, y].Find("Path/L").GetChild(0).gameObject.SetActive(false);
    }

    public void CheckQuestionMark(PathType pathType) {
        DisablePastRoomQuestion(pathType);
        DisableNowRoomQuestion();

        if(DungeonMap[PlayerIndex.x, PlayerIndex.y].GetComponent<Room>()._MonsterNum <= 0) {
            _uiManager.SetBattle(false);
        } else {
            _uiManager.SetBattle(true);
        }
    }

    private void DisablePastRoomQuestion(PathType pathType) {
        // 전 방 인덱스
        MapIndex pastRoomIndex = new MapIndex(PlayerIndex.x, PlayerIndex.y);
        switch (pathType) {
            case PathType.LEFT: pastRoomIndex.y++; break;
            case PathType.TOP: pastRoomIndex.x++; break;
            case PathType.RIGHT: pastRoomIndex.y--; break;
            case PathType.BOTTOM: pastRoomIndex.x--; break;
            default:
                break;
        }
        // 상하좌우 방 : 상->B 하->T 좌->R 우->L 지우기
        int x; int y;
        // 상
        x = pastRoomIndex.x - 1; y = pastRoomIndex.y;
        if (x >= 0) {
            Transform path = DungeonMap[x, y].Find("Path/B");
            path.GetChild(0).gameObject.SetActive(false); // 물음표
            path.GetComponent<MeshRenderer>().material.color = Color.red;
            path.GetComponent<DoorPath>().IsOpen = true;
        }
        // 하
        x = pastRoomIndex.x + 1; y = pastRoomIndex.y;
        if (x < DungeonMap.GetLength(0)) {
            Transform path = DungeonMap[x, y].Find("Path/T");
            path.GetChild(0).gameObject.SetActive(false);
            path.GetComponent<MeshRenderer>().material.color = Color.red;
            path.GetComponent<DoorPath>().IsOpen = true;

        }
        // 좌
        x = pastRoomIndex.x; y = pastRoomIndex.y - 1;
        if (y >= 0) {
            Transform path = DungeonMap[x, y].Find("Path/R");
            path.GetChild(0).gameObject.SetActive(false);
            path.GetComponent<MeshRenderer>().material.color = Color.red;
            path.GetComponent<DoorPath>().IsOpen = true;

        }
        // 우
        x = pastRoomIndex.x; y = pastRoomIndex.y + 1;
        if (y < DungeonMap.GetLength(1)) {
            Transform path = DungeonMap[x, y].Find("Path/L");
            path.GetChild(0).gameObject.SetActive(false);
            path.GetComponent<MeshRenderer>().material.color = Color.red;
            path.GetComponent<DoorPath>().IsOpen = true;
        }
    }
}

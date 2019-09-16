using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necronomicon : Grimoire {
    private NecronomiconBar _necronomiconBar;
    public GameObject[] _monsterPrefabs;

    private void Start() {
        _monsterPrefabs  = GameObject.Find("MonsterManager").GetComponent<MonsterManager>()._MonsterPrefabs;
        _necronomiconBar = GameObject.Find("NecronomiconBar").GetComponent<NecronomiconBar>();

        this.gameObject.SetActive(false);
    }

    public void SetMouseCursor() {
        if (_necronomiconBar.IsCapturing()) MouseCursor.Instance.Set(CursorIcon.kBasic);
        else MouseCursor.Instance.Set(CursorIcon.kCapture, 4);
    }

    public override void Attack() {
        if (!OperationData.TargetMonster) return;

        // 가진 스킬도 보내줄것 !, 파라미터 수정 예정
        if (_necronomiconBar.Capture(_playerStats, OperationData.TargetMonster.gameObject)) {
        }
    }
}

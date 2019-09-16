using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeDetectSensor : MonoBehaviour {
    public Transform _target;
    private Monster _monster;

    private void Start() {
        SetTarget();
    }
    private void OnTriggerEnter(Collider col) {
        if (!_monster) SetTarget();
        if (col.CompareTag("Player")){ // 플레이어 감지
            _monster.DoWork(MonsterComponentKind.SKILL, col.transform);
        }
        else if (col.CompareTag("Hero")) { //용사 감지
           _monster.DoWork(MonsterComponentKind.SKILL, col.transform);
        }
        else return;
    }
    private void OnTriggerExit(Collider col) {
        if (!_monster) SetTarget();
        if (col.CompareTag("Player")) { // 플레이어 감지
            _monster.DoWork(MonsterComponentKind.SKILL);
        }
        else if (col.CompareTag("Hero")) { //용사 감지
            _monster.DoWork(MonsterComponentKind.SKILL);
        }
        else return;
    }
    private void SetTarget() {
        if (_target.CompareTag("Monster")) {
            _monster = _target.GetComponent<Monster>();
        }
    }
}

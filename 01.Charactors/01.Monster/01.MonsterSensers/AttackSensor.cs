using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour {
    public Transform _target;
    private Monster _monster;

    private List<GameObject> _attackOjects = new List<GameObject>();
    private void Start() {
        _monster = _target.GetComponent<Monster>();
    }
    private void Update() {
        if (!_monster.gameObject.activeInHierarchy)
            _attackOjects.Clear();
        for (int i = 0; i < _attackOjects.Count; i++) {
            //타겟이 사망하면 공격불가능 
            if (_attackOjects[i].CompareTag("Untagged") ||
                 !_attackOjects[i].activeInHierarchy) {
                _monster.DoWork(MonsterComponentKind.ATTACK);
                _attackOjects.RemoveAt(i);
            }
            else
                _monster.DoWork(MonsterComponentKind.ATTACK, _attackOjects[i].transform);
        }
    }
    private void OnTriggerEnter(Collider col) {
        for (int i = 0; i < _attackOjects.Count; i++) {
            if (object.ReferenceEquals(_attackOjects[i], col.gameObject)) return;
        }
        if (col.CompareTag("Player") || col.CompareTag("Hero")) {
            // 플레이어 or 용사 감지
            _attackOjects.Add(col.gameObject);
        }
    }
    private void OnTriggerExit(Collider col) {
        if (col.CompareTag("Player") || col.CompareTag("Hero")) { 
            //용사 or 플레이어 감지
            _monster.DoWork(MonsterComponentKind.ATTACK);
            for (int i = 0; i < _attackOjects.Count; i++) {
                if (object.ReferenceEquals(_attackOjects[i], col.gameObject))
                    _attackOjects.RemoveAt(i);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSensor : MonoBehaviour {
    public Transform _target;
    private Monster _monster;
    private float _distance = 1000f;
    private int _closeNum;
    private List<GameObject> _detectOjects = new List<GameObject>();
    private void Start() {
        _monster = _target.GetComponent<Monster>();
    }
    private void Update() {
        for (int i = 0; i < _detectOjects.Count; i++) {
            //타겟이 사망하면 리스트에서 삭제
            if (_detectOjects[i].CompareTag("Untagged") ||
                !_detectOjects[i].activeInHierarchy                 
                ) { _detectOjects.RemoveAt(i); continue; }      
        }
        for (int i = 0; i < _detectOjects.Count; i++){
            // 둘 간의 거리 비교 후 가장 가까운 놈의 거리를 가져옴 
            if (_distance > Vector3.Distance(_monster.transform.position, _detectOjects[i].transform.position)) {
                _distance = Vector3.Distance(_monster.transform.position, _detectOjects[i].transform.position);
                _closeNum = i;
            }
        }
        _distance = 1000f;
        if (_detectOjects.Count == 0) return;
        
        _monster.SetTarget(_detectOjects[_closeNum].transform);
        _monster.DoWork(MonsterComponentKind.PATTERN, _detectOjects[_closeNum].transform);
    }

    private void OnDisable() {
        _detectOjects.Clear();
    }
    private void OnTriggerEnter(Collider col) {
        for (int i = 0; i < _detectOjects.Count; i++){
            if (object.ReferenceEquals(_detectOjects[i], col.gameObject)) return;
        }
        if (col.CompareTag("Player") || col.CompareTag("Hero")) { // 플레이어 or 용사 감지
            _detectOjects.Add(col.gameObject);
        }  
    }
    private void OnTriggerExit(Collider col) {
        if (col.CompareTag("Player")) { // 플레이어 감지
            _monster.DoWork(MonsterComponentKind.PATTERN);
        }
        else if (col.CompareTag("Hero")) { //용사 감지
              _monster.DoWork(MonsterComponentKind.PATTERN);
        }
    }
}

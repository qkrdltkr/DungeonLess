using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDetectSenSor : MonoBehaviour {
    public Transform _target;
    private Hero _hero;
    private float _distance = 1000f;
    private int _closeNum;
    private List<GameObject> _detectOjects = new List<GameObject>();
    private void Start() {
        _hero = _target.GetComponent<Hero>();
    }
    private void Update() {
        for (int i = 0; i < _detectOjects.Count; i++)  {
            //타겟이 사망하면 리스트에서 삭제
            if (_detectOjects[i].CompareTag("Untagged")) { _detectOjects.RemoveAt(i); continue; }
        }
        for (int i = 0; i < _detectOjects.Count; i++) {
            // 둘 간의 거리 비교 후 가장 가까운 놈의 거리를 가져옴 
            if (_distance > Vector3.Distance(_hero.transform.position, _detectOjects[i].transform.position)) {
                _distance = Vector3.Distance(_hero.transform.position, _detectOjects[i].transform.position);
                _closeNum = i;
            }
        }
        _distance = 1000f;
        if (_detectOjects.Count == 0) return;

        _hero.SetTarget(_detectOjects[_closeNum].transform);
        _hero.DoWork(HeroComponentKind.PATTERN, _detectOjects[_closeNum].transform);
    }
    private void OnTriggerEnter(Collider col) {
        for (int i = 0; i < _detectOjects.Count; i++) {
            if (object.ReferenceEquals(_detectOjects[i], col.gameObject)) return;
        }
        if (col.CompareTag("Player") || col.CompareTag("Monster")) { // 플레이어 or 몬스터 감지
            _detectOjects.Add(col.gameObject);
        }
    }
    private void OnTriggerExit(Collider col) {
        if (col.CompareTag("Player")) { // 플레이어 감지
            _hero.DoWork(HeroComponentKind.PATTERN);
        }
        else if (col.CompareTag("Monster")) { //몬스터 감지
            _hero.DoWork(HeroComponentKind.PATTERN);
        }
    }
}

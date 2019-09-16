using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackSenSor : MonoBehaviour {
    public Transform _target;
    private Hero _hero;

    private List<GameObject> _attackOjects = new List<GameObject>();
    private void Start() {
        _hero = _target.GetComponent<Hero>();
    }
    private void Update() {
        for (int i = 0; i < _attackOjects.Count; i++) {
            //타겟이 사망하면 공격불가능 
            if (_attackOjects[i].CompareTag("Untagged")) {
                _hero.DoWork(HeroComponentKind.ATTACK);
                _attackOjects.RemoveAt(i); 
            }
            else
                _hero.DoWork(HeroComponentKind.ATTACK, _attackOjects[i].transform);
        }
    }
    private void OnTriggerEnter(Collider col) {
        
        for (int i = 0; i < _attackOjects.Count; i++) {
            if (object.ReferenceEquals(_attackOjects[i], col.gameObject)) return;
        }
        if (col.CompareTag("Player") || col.CompareTag("Monster")) {
            // 플레이어 or 몬스터 감지
            _attackOjects.Add(col.gameObject);
        }

    }
    private void OnTriggerExit(Collider col) {
        if (col.CompareTag("Player") || col.CompareTag("Monster")) {
            // 플레이어 or 몬스터 감지
            _hero.DoWork(HeroComponentKind.ATTACK);
            for (int i = 0; i < _attackOjects.Count; i++) {
                if (object.ReferenceEquals(_attackOjects[i], col.gameObject))
                    _attackOjects.RemoveAt(i);
            }
        }
    }
}

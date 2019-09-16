using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePattenComponent : PatternComponent {

    public override void Run(float dt) {
        if (!_target) return;
        if (_isTracing) Trace(dt, true);
    }
    public override void SetTarget(Transform target){
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _attackSensor = _monster.Find("AttackSensor");
        _monsterStats = _monster.GetComponent<Monster>().GetStats();
        _condition = condition;
    }

    public override void DoWork(Transform tr) {
        if (!tr) return;
        if (tr.CompareTag("Player") || tr.CompareTag("Hero")) {
            _isTracing = true;
        } //추격가능
    }
}

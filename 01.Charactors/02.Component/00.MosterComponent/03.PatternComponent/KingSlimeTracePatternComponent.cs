using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeTracePatternComponent : PatternComponent {
    public override void Run(float dt) {
        if (!_target || !_monster) return;
        if (_isTracing) Trace(dt, true);
    }
    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _attackSensor = _monster.Find("AttackSensor");
        _condition = condition;
    }
    public override void DoWork(Transform tr) {
        if (!_target) return;
        if (_target.CompareTag("Player") || _target.CompareTag("Hero")) {
            _isTracing = true;
        } //추격가능
    }
}

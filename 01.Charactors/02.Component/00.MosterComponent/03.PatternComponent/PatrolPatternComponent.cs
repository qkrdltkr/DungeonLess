﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPatternComponent : PatternComponent {
    public override void Run(float dt) {
        if (!_target) {
            Patrol(dt, Condition.kRun); return;
        }
        if (_isTracing) Trace(dt, true);
        else Patrol(dt, Condition.kRun);
    }

    public override void SetTarget(Transform target) {
        Debug.Log("감지");
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _attackSensor = _monster.Find("AttackSensor");
        _condition = condition;
        SetPatrolInit();
    }

    public override void DoWork(Transform tr) {
        if (!tr) return;
        if (tr.CompareTag("Player") || tr.CompareTag("Hero")) {
            _isTracing = true;
        } //추격가능
    }  
}

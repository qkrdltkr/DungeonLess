using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunawayPatternComponent : PatternComponent {
    private float _durationTime;
    public override void Run(float dt) {
        if (!_target) {
            _isRunaway = false;
            Patrol(dt, Condition.kIdle);
            return;
        }
        if (_condition.currentCondition == Condition.kDead) return;
        if (_condition.currentCondition == Condition.kAttack) {
            _durationTime += dt;
            if (_durationTime < 0.5f) return;
        }
        _condition.SetCondition(Condition.kIdle);
        _durationTime -= dt;
        if (_durationTime <= 0) {
            _durationTime = 0;
            if (_isRunaway) Runaway(dt);
            else Patrol(dt, Condition.kIdle);
        }
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
        InitRandomLocation();
    }

    public override void DoWork(Transform tr) {
        if (tr == null) _isRunaway = false;
        else if (tr.CompareTag("Player") || tr.CompareTag("Hero")) {
            _isRunaway = true;  //도망가능
        } 
    } 
}

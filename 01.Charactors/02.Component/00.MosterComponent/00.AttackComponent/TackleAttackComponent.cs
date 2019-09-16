using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleAttackComponent : AttackComponent {
    private float _delayTime;
    public override void Run(float dt) {
        RecoveryMana(dt);
        if (!_target) return;
        if (_isMonsterCanAttack) {
            _delayTime += dt;
            if(_delayTime > _monsterStats.ShootSpeed){
                MeleeAttack();
                _delayTime = 0;
            }
        }
        else {
            _condition.SetCondition(Condition.kIdle);
        }
    }

    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _condition = condition;
    }
    public override void DoWork(Transform tr) {
        if (!tr) return;
        if (tr.CompareTag("Player") || tr.CompareTag("Hero")) {
            _isMonsterCanAttack = true; //타겟이 공격범위안에 들어옴
        } 
        //공격가능
    }

    public override void MeleeAttack() {
        float _distance = Vector3.Distance(_monster.position, _target.position);
        if (_distance < 2f) {
            if (_target.CompareTag("Player"))
                _target.GetComponent<PlayerController>().Damaged(Utility.LookAt(_target, _monster) *
                    _monsterStats.PushPower, _monsterStats.Damage, StatusEffectKind.kNoStatusEffect);
            else if (_target.CompareTag("Hero"))
                _target.GetComponentInChildren<Hero>().DoWork(HeroComponentKind.DAMAGED, _hitBox);
        }
        else
            _isMonsterCanAttack = false;
    }
}
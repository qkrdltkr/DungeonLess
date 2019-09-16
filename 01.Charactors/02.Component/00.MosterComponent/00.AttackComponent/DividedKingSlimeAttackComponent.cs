using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividedKingSlimeAttackComponent : AttackComponent {
 public DividedKingSlimeAttackComponent(MonsterSkillComponent monsterSkillComponent) {
        _monsterSkillComponent = (DividedKingSlimeSkillComponent)monsterSkillComponent;
        monsterSkillComponent.LearnKingSlimeSkill(_monsterStats, _monster);
    }
    private float _delayTime;
    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _condition = condition;
    }

    public override void Run(float dt) {
        //마나 회복
        RecoveryMana(dt);
        if (!_target) return;
        //근접 공격
        MeleeAttack();
        //스킬 사용
        UseSkill(dt);
    }

    public override void MeleeAttack() {
        //근접공격
        if (_isMonsterCanAttack) {
            if (_target.CompareTag("Player"))
                _target.GetComponent<PlayerController>().Damaged(Utility.LookAt(_target, _monster) *
                    _monsterStats.PushPower, _monsterStats.Damage, StatusEffectKind.kNoStatusEffect);
            else if (_target.CompareTag("Hero"))
                _target.GetComponent<Hero>().Damaged(_monster);

            _isMonsterCanAttack = false;
        }
    }
    private void UseSkill(float dt) {
        _delayTime += dt;
        if(_delayTime >= _monsterStats.ShootSpeed) {
            if (_monsterStats.ManaPoint >= 100f && (
                _condition.currentCondition.Equals(Condition.kIdle) ||
                _condition.currentCondition.Equals(Condition.kRun))) {
                // 조건에 맞으면 스킬
                if (_monsterSkillComponent.UseSkill()){
                    Debug.Log("킹슬라임 스킬 발싸 ");
                    _monsterStats.ManaPoint -= 100f;
                }
            }
            _delayTime = 0;
        }
    }

    public override void DoWork(Transform tr) {
        if (tr == null) return;
        if (tr.CompareTag("Player") || tr.CompareTag("Hero")) {
            _isMonsterCanAttack = true;
        }
        //공격가능
    }
}

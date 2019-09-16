using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackComponent : AttackComponent {
    public MeleeAttackComponent(MonsterSkillComponent monsterSkillComponent){
        ClassifyComponent(monsterSkillComponent);
    }
    private int _randomSkillCount = 5;
    float AttackDelayTime = 0;

    private bool isAttacking;
    private Transform _attackcol;
   
    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _condition = condition;
        _attackcol = _monster.transform.Find("AttackSensor/Sword/SwordSensor");
        _SwordAnimator = _monster.GetChild(1).GetChild(0).GetComponent<Animator>();
    }

    public override void Run(float dt) {
        RecoveryMana(dt);
        if (!_target){
            return;
        }
        DelayAttack(dt);
        CalculateDamage();
        PlayAnimaiton(dt);
    }
    private void CalculateDamage() {
        if (_attackcol.GetComponent<CreateAttackCol>().IsPlayerInAttackCol && _target.CompareTag("Player")) { //플레이어 공격
            _target.GetComponentInChildren<PlayerController>().Damaged(Utility.LookAt(_target, _monster) *
                _monsterStats.PushPower, _monsterStats.Damage, StatusEffectKind.kNoStatusEffect);
            _attackcol.GetComponent<CreateAttackCol>().IsPlayerInAttackCol = false;
        }  else if (_attackcol.GetComponent<CreateAttackCol>().IsHeroInAttackCol && _target.CompareTag("Hero"))  { //용사 공격
            _target.GetComponentInChildren<Hero>().DoWork(HeroComponentKind.DAMAGED, _hitBox);
            _attackcol.GetComponent<CreateAttackCol>().IsHeroInAttackCol = false;
        }
    }
    private void PlayAnimaiton(float dt) {
        if (isAttacking) {
            AttackDelayTime += dt;
            if (AttackDelayTime > _monsterStats.ShootSpeed) {
                _condition.SetCondition(Condition.kIdle);
                AttackDelayTime = 0f;
                isAttacking = false;
            }
        }
    }

    private void DelayAttack(float dt) {
        if (_isMonsterCanAttack) { //공격 가능일때 
            if (_currentTime < dt) MeleeAttack();//일단 휘두름
            _currentTime += dt;
            if (_currentTime > _monsterStats.ShootSpeed) {
                if (_randomSkillCount - Random.Range(0, _randomSkillCount) == 1 &&
                    _monsterStats.ManaPoint >= 100f) { // 조건에 맞으면 스킬
                    if (_monsterSkillComponent.UseSkill()) {
                        _randomSkillCount = 5; //스킬빈도수 (5분의1)
                        _monsterStats.ManaPoint -= 100f;
                    }
                    else MeleeAttack(); //스킬을 배우지 않았으면 평타
                }
                else MeleeAttack(); // 기본평타
                _currentTime = 0.0f;
            }
        }
    }
    public override void DoWork(Transform tr) {
        if (!tr) _isMonsterCanAttack = false;//타겟이 공격범위를 벗어남
        else if (tr.CompareTag("Player") || tr.CompareTag("Hero")) {
            if (_target == tr) _isMonsterCanAttack = true; 
        }
    }
    public override void MeleeAttack() {
        isAttacking = true;
        _attackcol.gameObject.SetActive(true);
        
        _condition.SetCondition(Condition.kAttack);
    }
}

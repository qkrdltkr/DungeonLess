using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMeleeAttackComponent : HeroAttackComponent {

    private float _attackDelayTime = 0;
    private float distance;
    private bool _isAttacking;
    private Transform _attackcol;

    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _hero = origin;
        _hitBox = _hero.Find("HitBox");
        _heroStats = _hitBox.GetComponent<Hero>().GetStats();
        _condition = condition;
        _attackcol = _hero.transform.Find("AttackSensor/Sword/SwordSensor");
        _SwordAnimator = _hero.GetChild(1).GetChild(0).GetComponent<Animator>();
    }

    public override void Run(float dt) {
        RecoveryMana(dt);
        //공격이 끝나면 idle로 바꿔줌
        if (_condition.currentCondition.Equals(Condition.kAttack) && !_target) {
            _condition.SetCondition(Condition.kIdle);
        }
        //타겟이 없으면 리턴
        if (!_target) return;
        DelayAttack(dt);
        CalculateDamage();
        PlayAnimaiton(dt);
    }
    private void CalculateDamage() {
        if (_attackcol.GetComponent<CreateAttackCol>().IsPlayerInAttackCol && _target.CompareTag("Player")) { //플레이어 공격
            _target.GetComponentInChildren<PlayerController>().Damaged(Utility.LookAt(_target, _hero) *
                _heroStats.PushPower, _heroStats.Damage, StatusEffectKind.kNoStatusEffect);
            _attackcol.GetComponent<CreateAttackCol>().IsPlayerInAttackCol = false;
        } else if (_attackcol.GetComponent<CreateAttackCol>().IsMonsterInAttackCol && _target.CompareTag("Monster")) { //몬스터 공격
            _target.GetComponentInChildren<Monster>().DoWork(MonsterComponentKind.DAMAGED, _hitBox);
            _attackcol.GetComponent<CreateAttackCol>().IsMonsterInAttackCol = false;
        }
    }
    private void PlayAnimaiton(float dt) {
        if (_isAttacking) {
            _attackDelayTime += dt;
            if (_attackDelayTime > _heroStats.ShootSpeed) {
                _condition.SetCondition(Condition.kIdle);
                _attackDelayTime = 0f;
                _isAttacking = false;
            }
        }
    }
    private void DelayAttack(float dt) {
        if (_isHeroCanAttack) { //공격 가능일때 
            if (_currentTime < dt) MeleeAttack();//일단 휘두름
            _currentTime += dt;
            if (_currentTime > _heroStats.ShootSpeed) {
                Debug.Log("히어로 평타!");
                 MeleeAttack(); // 기본평타
                _currentTime = 0.0f;
            }
        }
    }
    public override void DoWork(Transform tr) {
        if (!tr) _isHeroCanAttack = false;//타겟이 공격범위를 벗어남
        else if (tr.CompareTag("Monster") || tr.CompareTag("Player")) {
            if (_target == tr) _isHeroCanAttack = true;
        }
    }
    public override void MeleeAttack() {
        _isAttacking = true;
        _attackcol.gameObject.SetActive(true);
       
        _condition.SetCondition(Condition.kAttack);
    }
}

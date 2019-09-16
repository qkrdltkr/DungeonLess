using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiateAttackComponent : AttackComponent {
    public RadiateAttackComponent(MonsterSkillComponent monsterSkillComponent) {
        ClassifyComponent(monsterSkillComponent);
    }

    private int _randomSkillCount = 5;
    private float _durationTime;
    private bool isAttacking;
    private Transform attackcol;

    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition)  {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _condition = condition;
    }
   
    public override void Run(float dt) {
        DelayAttack(dt);
        RecoveryMana(dt);
    }

    private void DelayAttack(float dt) {
        if (_isMonsterCanAttack) { //공격 가능일때 
            _currentTime += dt;
            if (_currentTime > _monsterStats.ShootSpeed) {
                if (_randomSkillCount - Random.Range(0, _randomSkillCount) == 1 &&
                    _monsterStats.ManaPoint >= 100f)
                { // 조건에 맞으면 스킬
                    if (_monsterSkillComponent.UseSkill()) {
                        _randomSkillCount = 5; //스킬빈도수 (5분의1)
                        _monsterStats.ManaPoint -= 100f;
                    }
                    else RadiateAttack(); //스킬을 배우지 않았으면 방사
                }
                else RadiateAttack(); // 기본방사
                _currentTime = 0.0f;
            }
        }
    }
    public override void DoWork(Transform tr) {
        _isMonsterCanAttack = true;//플레이어가 공격범위안에 들어옴
    }
    public override void RadiateAttack() {
        _condition.SetCondition(Condition.kAttack);
        GameObject bullet = MonsterBulletPoolManager.Instance.PopBullet(MonsterBulletKind.kRadiate);
        RadialBullet radiateBullet = bullet.GetComponent<RadialBullet>();
        radiateBullet.Fire(_monsterStats, _monster);
    }
}

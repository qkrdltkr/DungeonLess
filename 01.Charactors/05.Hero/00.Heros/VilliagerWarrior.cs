using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilliagerWarrior : Hero {
    Transform _VilliagerTr;
    Transform[] _PatrolPoss;
    protected override void Initialize() {
        //스탯 설정
        SetHeroStats();
        //애니메이션 컴포넌트
        SetAnimationComponent();
        //AI 컴포넌트
        SetAiComponent();
        //히어로 기본세팅
        HeroInitalize();
    }

    private void HeroInitalize() {
        _VilliagerTr = this.transform.parent.transform;
        Target = OperationData.Player;
        for (int i = 0; i < _heroAiComponents.Length; ++i){
            _heroAiComponents[i].Initialize(_VilliagerTr, GetCurrentCondition());
        }
    }
    private void SetAnimationComponent() {
        _currentCondition.SetCondition(Condition.kIdle);
        _animationComponent = new HeroAnimationComponent(this.transform.GetComponent<Animator>(),
            this.transform.parent.GetChild(1).GetChild(0).GetComponent<Animator>(),
            new string[] { "IsIdle", "IsMove", "IsDead", "IsAttack"});
        _animationComponent.CurrentCondition = _currentCondition;
    }
    private void SetAiComponent() {
        //AI 컴포넌트
        _heroAiComponents[0] = new HeroTracePatternComponent(); //순찰
        _heroAiComponents[1] = new HeroMeleeAttackComponent();   //근접공격
        _heroAiComponents[2] = new HeroDamagedComponent();       //맞기
    }
    private void SetHeroStats() {
        _characterStats.MoveSpeed = 1f;
        _characterStats.ShootSpeed = 0.5f;
        _characterStats.BulletSpeed = 0f;
        _characterStats.Damage = 5;
        _characterStats.PushPower = 20f;
        _characterStats.Range = 0f;
        _characterStats.ResistancePoint = 1.0f;
        _characterStats.CurrentExp = 40;
        _characterStats.MaxExp = 100;
        _characterStats.Level = 1;
        _characterStats.MaxHeatPoint = 100;
        _characterStats.MaxManaPoint = 100;
        _characterStats.HeatPoint = 100;
        _characterStats.ManaRecovery = 10f;
        _characterStats.ManaPoint = 0f;
        _characterStats.Name = CName.kVilliagerWarrior[Random.Range(0, CName.kVilliagerWarrior.Length)];
        _characterStats.Description = CDescription.kVilliagerWarrior;
        _characterStats.Population = 1;
    }
}

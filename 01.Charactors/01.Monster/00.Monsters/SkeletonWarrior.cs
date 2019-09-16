using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : Monster {
    private Transform _skeletonWarriorTr;
    private Transform[] _patrolPoss;
    protected override void Initialize() {
        Index = new MapIndex();
        //스탯 설정
        SetMonsterStats();
        //애니메이션 컴포넌트
        SetAnimationComponent();
        //AI 컴포넌트
        SetAiComponent();
        //몬스터 기본세팅
        MonsterInitalize();
    }

    private void MonsterInitalize() {
        this._Kind = MonsterKind.SKELETONWARRIOR;

        _skeletonWarriorTr = this.transform.parent.transform;
        Target = OperationData.Player;
        for (int i = 0; i < _monsterAiComponents.Length; ++i) {
            _monsterAiComponents[i].Initialize(_skeletonWarriorTr, _currentCondition);
        }
    }
    private void SetAnimationComponent() {
        _currentCondition.SetCondition(Condition.kIdle);
        _animationComponent = new TwoWayWeaponAnimationComponent(this.transform.GetComponent<Animator>(),
            this.transform.parent.GetChild(1).GetChild(0).GetComponent<Animator>(),
             new string[] { "Attack", "Walk", "Dead", "Idle"}
            );
        _animationComponent.CurrentCondition = _currentCondition;
    }
    private void SetAiComponent() {
        //스킬AI 컴포넌트
        switch (_Attributes) {
            case ElementalAttributes.FLAME:
                _monsterAiComponents[3] = new FlameSkillComponent(); break;
            case ElementalAttributes.WATER:
                _monsterAiComponents[3] = new WaterSkillComponent(); break;
            case ElementalAttributes.NATURE:
                _monsterAiComponents[3] = new NatureSkillComponent(); break;
            case ElementalAttributes.LIGHT:
                _monsterAiComponents[3] = new LightSkillComponent(); break;
            case ElementalAttributes.DARKNESS:
                _monsterAiComponents[3] = new DarknessSkillComponent(); break;
        }

        //AI 컴포넌트
        _monsterAiComponents[0] = new PatrolPatternComponent(); //순찰
        _monsterAiComponents[1] = new MeleeAttackComponent((MonsterSkillComponent)_monsterAiComponents[3]);   //근접공격
        _monsterAiComponents[2] = new DamagedComponent();       //맞기
    }
    private void SetMonsterStats() {
        _characterStats.MoveSpeed = 1f;
        _characterStats.ShootSpeed = 0.5f;
        _characterStats.BulletSpeed = 0f;
        _characterStats.Damage = 5;
        _characterStats.PushPower = 20f;
        _characterStats.Range = 0f;
        _characterStats.ResistancePoint = 40.0f;
        _characterStats.CurrentExp = 40;
        _characterStats.MaxExp = 100;
        _characterStats.Level = 1;
        _characterStats.MaxHeatPoint = 100;
        _characterStats.MaxManaPoint = 100;
        _characterStats.HeatPoint = 100;
        _characterStats.ManaRecovery = 10f;
        _characterStats.ManaPoint = 0f;
        _characterStats.Population = 1;
        _characterStats.Name = CName.kSkeltonWarrior;
        _characterStats.Description = CDescription.kSkeltonWarrior;
        _characterStats.Tribe = MonsterTribe.UNDEAD;
    }
}

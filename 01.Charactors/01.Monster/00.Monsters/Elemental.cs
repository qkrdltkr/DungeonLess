using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elemental : Monster {
    Transform _NatureElementalTr;
    Transform[] _PatrolPoss;
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
        this._Kind = MonsterKind.ICE_ELEMENTAL;
        Target = OperationData.Player;
        _NatureElementalTr = this.transform.parent.transform;
        for (int i = 0; i < _monsterAiComponents.Length; ++i) {
            _monsterAiComponents[i].Initialize(_NatureElementalTr, _currentCondition);
        }
    }
    private void SetAnimationComponent() {
        _currentCondition.SetCondition(Condition.kIdle);
        _animationComponent = new TwoWayAnimationComponent(this.transform.GetComponent<Animator>(),
            new string[] { "Idle", "Attack", "Dead"}
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
        _monsterAiComponents[0] = new RunawayPatternComponent();
        _monsterAiComponents[1] = new RadiateAttackComponent((MonsterSkillComponent)_monsterAiComponents[3]);
        _monsterAiComponents[2] = new DamagedComponent();
    }

    private void SetMonsterStats() {
        _characterStats.MoveSpeed = 1f;
        _characterStats.ShootSpeed = 2f;
        _characterStats.BulletSpeed = 2f;
        _characterStats.Damage = 10;
        _characterStats.PushPower = 0f;
        _characterStats.Range = 3f;
        _characterStats.ResistancePoint = 20.0f;
        _characterStats.CurrentExp = 10;
        _characterStats.MaxExp = 100;
        _characterStats.Level =5;
        _characterStats.MaxHeatPoint = 80;
        _characterStats.MaxManaPoint = 100;
        _characterStats.HeatPoint = 80;
        _characterStats.ManaRecovery = 1f;
        _characterStats.ManaPoint = 0f;
        _characterStats.Population = 2;
        _characterStats.Name = CName.kElemental;
        _characterStats.Description = CDescription.kElemental;
        _characterStats.Tribe = MonsterTribe.ELF;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividedKingSlime : Monster{
    Transform _KingSlimeTr;
    Transform[] _PatrolPoss;
    protected override void Initialize() {
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
        Target = OperationData.Player;
        _KingSlimeTr = this.transform.parent.transform;
        for (int i = 0; i < _monsterAiComponents.Length; ++i) {
            _monsterAiComponents[i].Initialize(_KingSlimeTr, _currentCondition);
        }
    }
    private void SetAnimationComponent() {
        _currentCondition.SetCondition(Condition.kIdle);
        _animationComponent = new KingSlimeAnimationComponent(this.transform.GetComponent<Animator>(),
            new string[] { "Spit", "Split", "Jump", "Dead", "Walk", "Idle" });
        _animationComponent.CurrentCondition = _currentCondition;
    }
    private void SetAiComponent() {
        //스킬AI 컴포넌트
        _monsterAiComponents[3] = new DividedKingSlimeSkillComponent();

        //AI 컴포넌트
        _monsterAiComponents[0] = new KingSlimeTracePatternComponent();
        _monsterAiComponents[1] = new DividedKingSlimeAttackComponent((MonsterSkillComponent)_monsterAiComponents[3]);
        _monsterAiComponents[2] = new DamagedComponent();
    }

    private void SetMonsterStats()
    {
        _characterStats.MoveSpeed = 1f;
        _characterStats.ShootSpeed = 1f;
        _characterStats.BulletSpeed = 0f;
        _characterStats.Damage = 10;
        _characterStats.PushPower = 0f;
        _characterStats.Range = 5f;
        _characterStats.ResistancePoint = 10.0f;
        _characterStats.CurrentExp = 10;
        _characterStats.MaxExp = 100;
        _characterStats.Level = 1;
        _characterStats.MaxHeatPoint = 300;
        _characterStats.MaxManaPoint = 100;
        _characterStats.HeatPoint = 300;
        _characterStats.ManaRecovery = 10f;
        _characterStats.ManaPoint = 100f;
        _characterStats.Name = CName.kSlime;
        _characterStats.Description = CDescription.kSlime;
    }

}

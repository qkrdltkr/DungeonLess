using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonsterAIComponent {
    protected float _currentTime = 0;
    protected float _recoveryTime = 0;
    protected bool _isMonsterCanAttack;
    protected MonsterSkillComponent _monsterSkillComponent;
    // ~~~~ AttackComponent들의 부모
    public override void Clear() {
        _isMonsterCanAttack = false;
        _target = null;
    }
    public override void SetTarget(Transform target) { }
    public override void Initialize(Transform origin, Condition condition) { }
    public virtual void MeleeAttack() { }

    public virtual void RadiateAttack() { }

    protected void RecoveryMana(float dt) {
        _recoveryTime += dt;
        if (_recoveryTime > 0.1f) {
            _monsterStats.ManaPoint += _monsterStats.ManaRecovery;
            if (_monsterStats.MaxManaPoint < _monsterStats.ManaPoint) {
                _monsterStats.ManaPoint = _monsterStats.MaxManaPoint;
            }
            _recoveryTime = 0.0f;
        }
    }
    protected void ClassifyComponent(MonsterSkillComponent monsterSkillComponent) {
        if (monsterSkillComponent is FlameSkillComponent) {
            _monsterSkillComponent = (FlameSkillComponent)monsterSkillComponent;
        }
        else if (monsterSkillComponent is WaterSkillComponent) {
            _monsterSkillComponent = (WaterSkillComponent)monsterSkillComponent;
        }
        else if (monsterSkillComponent is NatureSkillComponent) {
            _monsterSkillComponent = (NatureSkillComponent)monsterSkillComponent;
        }
        else if (monsterSkillComponent is LightSkillComponent) {
            _monsterSkillComponent = (LightSkillComponent)monsterSkillComponent;
        }
        else if (monsterSkillComponent is DarknessSkillComponent) {
            _monsterSkillComponent = (DarknessSkillComponent)monsterSkillComponent;
        }
    }
}

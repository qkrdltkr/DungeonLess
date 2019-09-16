using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackComponent : HeroAIComponent {
    protected float _currentTime = 0;
    protected float _recoveryTime = 0;
    protected MonsterSkillComponent _monsterSkillComponent;
    // ~~~~ HeroAttackComponent들의 부모
    public override void SetTarget(Transform target) { }
    public override void Initialize(Transform origin, Condition condition) { }
    public virtual void MeleeAttack() { }
    public override void Clear() {
        _isHeroCanAttack = false;
        _target = null;
    }
    protected void RecoveryMana(float dt)
    {
        _recoveryTime += dt;
        if (_recoveryTime > 0.1f)
        {
            _heroStats.ManaPoint += _heroStats.ManaRecovery;
            if (_heroStats.MaxManaPoint < _heroStats.ManaPoint)
            {
                _heroStats.ManaPoint = _heroStats.MaxManaPoint;
            }
            _recoveryTime = 0.0f;
        }
    }
}

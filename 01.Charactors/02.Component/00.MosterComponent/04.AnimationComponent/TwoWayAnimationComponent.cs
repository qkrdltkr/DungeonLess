using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayAnimationComponent : AnimationComponent{
    public TwoWayAnimationComponent(Animator anim, string[] conditions) : base(anim) {
        _conditions = conditions;
    }
    public override void Run(float dt) {
        if (!_animator.gameObject.activeInHierarchy) return;
        switch (CurrentCondition.currentCondition) {
            case Condition.kIdle:   ConvertCondition("Idle");break;
            case Condition.kDead:    ConvertCondition("Dead");  break;
            case Condition.kAttack:  ConvertCondition("Attack"); break;
        }
    }
}

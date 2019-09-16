using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum KingSlimeSkillKInds { NONPASS =-1, SPIT, SPLIT, JUMP}
public class KingSlimeAnimationComponent : AnimationComponent {
    public KingSlimeAnimationComponent(Animator anim, string[] conditions) : base(anim) {
        _conditions = conditions;
    }
    public override void Run(float dt) {
        if (!_animator.gameObject.activeInHierarchy) return;
        switch (CurrentCondition.currentCondition) {
            case Condition.kIdle: ConvertCondition("Idle"); break;
            case Condition.kRun:  ConvertCondition("Walk"); break; 
            case Condition.kDead: ConvertCondition("Dead"); break;
            case Condition.kAttack: {
                    switch (CurrentCondition.conditionKind) {
                        case (int)KingSlimeSkillKInds.SPIT:   ConvertCondition("Spit");  break;
                        case (int)KingSlimeSkillKInds.SPLIT:  ConvertCondition("Split"); break;
                        case (int)KingSlimeSkillKInds.JUMP:   ConvertCondition("Jump");  break;
                    }
                    break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayWeaponAnimationComponent : AnimationComponent {

    public TwoWayWeaponAnimationComponent(Animator monsterAnim, Animator weaponAnim, string[] conditions) : base(monsterAnim) {
        _weaponAnimator = weaponAnim;
        _conditions = conditions;
    }
    public override void Run(float dt) {
        if (!_animator.gameObject.activeInHierarchy) return;
        switch (CurrentCondition.currentCondition) {
            case Condition.kIdle: {
                    ConvertCondition("Idle");
                    _weaponAnimator.SetFloat("Swing", 0f);
                    _weaponAnimator.SetFloat("Idle", 1f);
                    break;
            }
            case Condition.kRun: {
                    ConvertCondition("Walk");
                    _weaponAnimator.SetFloat("Swing", 0f);
                    _weaponAnimator.SetFloat("Idle", 1f);
                    break;
            }
            case Condition.kAttack: {
                    ConvertCondition("Attack");
                    _weaponAnimator.SetFloat("Swing",1f);
                    _weaponAnimator.SetFloat("Idle", 0f);
                    break;
            }
            case Condition.kDead:{
                    ConvertCondition("Dead");
                    _weaponAnimator.SetFloat("Swing", 0f);
                    _weaponAnimator.SetFloat("Idle", 0f);
                    break;
                }

        }
    }
}
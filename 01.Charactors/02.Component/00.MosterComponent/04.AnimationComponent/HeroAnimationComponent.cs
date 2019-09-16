using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationComponent : AnimationComponent
{
   
    public HeroAnimationComponent(Animator monsterAnim, Animator weaponAnim, string[] conditions) : base(monsterAnim) {
        _weaponAnimator = weaponAnim;
        _conditions = conditions;
    }
   
    public override void Run(float dt) {
        if (!_animator.gameObject.activeInHierarchy) return;
        SetDir();
        SetState();
        SetWeaponState();
        switch (CurrentCondition.currentCondition) {
            case Condition.kIdle: { 
                switch (_lookDireation) {
                        case LookDireation.DEGREE_0:
                            _animator.SetFloat("IdlePosX", 0);
                            _animator.SetFloat("IdlePosY", 1); break;
                        case LookDireation.DEGREE_45:
                            _animator.SetFloat("IdlePosX", -1);
                            _animator.SetFloat("IdlePosY", 1); break;
                        case LookDireation.DEGREE_90:
                            _animator.SetFloat("IdlePosX", -1);
                            _animator.SetFloat("IdlePosY", 0); break;
                        case LookDireation.DEGREE_135:
                            _animator.SetFloat("IdlePosX", -1);
                            _animator.SetFloat("IdlePosY", -1); break;
                        case LookDireation.DEGREE_180:
                            _animator.SetFloat("IdlePosX", 0);
                            _animator.SetFloat("IdlePosY", -1); break;
                        case LookDireation.DEGREE_225:
                            _animator.SetFloat("IdlePosX", 1);
                            _animator.SetFloat("IdlePosY", -1); break;
                        case LookDireation.DEGREE_270:
                            _animator.SetFloat("IdlePosX", 1);
                            _animator.SetFloat("IdlePosY", 0); break;
                        case LookDireation.DEGREE_315:
                            _animator.SetFloat("IdlePosX", 1);
                            _animator.SetFloat("IdlePosY", 1); break;
                    }
                    break;
            }
            case Condition.kRun: {
                    switch (_lookDireation)  {
                        case LookDireation.DEGREE_0:
                            _animator.SetFloat("MovePosX", 0);
                            _animator.SetFloat("MovePosY", 1); break;
                        case LookDireation.DEGREE_45:
                            _animator.SetFloat("MovePosX", -1);
                            _animator.SetFloat("MovePosY", 1); break;
                        case LookDireation.DEGREE_90:
                            _animator.SetFloat("MovePosX", -1);
                            _animator.SetFloat("MovePosY", 0); break;
                        case LookDireation.DEGREE_135:
                            _animator.SetFloat("MovePosX", -1);
                            _animator.SetFloat("MovePosY", -1); break;
                        case LookDireation.DEGREE_180:
                            _animator.SetFloat("MovePosX", 0);
                            _animator.SetFloat("MovePosY", -1); break;
                        case LookDireation.DEGREE_225:
                            _animator.SetFloat("MovePosX", 1);
                            _animator.SetFloat("MovePosY", -1); break;
                        case LookDireation.DEGREE_270:
                            _animator.SetFloat("MovePosX", 1);
                            _animator.SetFloat("MovePosY", 0); break;
                        case LookDireation.DEGREE_315:
                            _animator.SetFloat("MovePosX", 1);
                            _animator.SetFloat("MovePosY", 1); break;
                    }
                    break;
                }
            case Condition.kAttack: {
                    switch (_lookDireation) {
                        case LookDireation.DEGREE_0:
                            _animator.SetFloat("AttackPosX", 0);
                            _animator.SetFloat("AttackPosY", 1);
                            _weaponAnimator.SetFloat("AttackPosX", 0);
                            _weaponAnimator.SetFloat("AttackPosY", 1); break;
                        case LookDireation.DEGREE_45:
                            _animator.SetFloat("AttackPosX", -1);
                            _animator.SetFloat("AttackPosY", 1);
                            _weaponAnimator.SetFloat("AttackPosX", -1);
                            _weaponAnimator.SetFloat("AttackPosY", 1); break;
                        case LookDireation.DEGREE_90:
                            _animator.SetFloat("AttackPosX", -1);
                            _animator.SetFloat("AttackPosY", 0);
                            _weaponAnimator.SetFloat("AttackPosX", -1);
                            _weaponAnimator.SetFloat("AttackPosY", 0); break;
                        case LookDireation.DEGREE_135:
                            _animator.SetFloat("AttackPosX", -1);
                            _animator.SetFloat("AttackPosY", -1);
                            _weaponAnimator.SetFloat("AttackPosX", -1);
                            _weaponAnimator.SetFloat("AttackPosY", -1); break;
                        case LookDireation.DEGREE_180:
                            _animator.SetFloat("AttackPosX", 0);
                            _animator.SetFloat("AttackPosY", -1);
                            _weaponAnimator.SetFloat("AttackPosX", 0);
                            _weaponAnimator.SetFloat("AttackPosY", -1); break;
                        case LookDireation.DEGREE_225:
                            _animator.SetFloat("AttackPosX", 1);
                            _animator.SetFloat("AttackPosY", -1);
                            _weaponAnimator.SetFloat("AttackPosX", 1);
                            _weaponAnimator.SetFloat("AttackPosY", -1); break;
                        case LookDireation.DEGREE_270:
                            _animator.SetFloat("AttackPosX", 1);
                            _animator.SetFloat("AttackPosY", 0);
                            _weaponAnimator.SetFloat("AttackPosX", 1);
                            _weaponAnimator.SetFloat("AttackPosY", 0); break;
                        case LookDireation.DEGREE_315:
                            _animator.SetFloat("AttackPosX", 1);
                            _animator.SetFloat("AttackPosY", 1);
                            _weaponAnimator.SetFloat("AttackPosX", 1);
                            _weaponAnimator.SetFloat("AttackPosY", 1); break;
                    }
                    break;
            }
            case Condition.kDead: {
                    ConvertConditionBool("IsDead");
                    break;
            }
        }
    }
    private void SetDir() {
        if (!_origin) return;
        float degree = Utility.GetAngle(_origin.transform.position, _targetpos);
        if (degree > 0.0f && degree <= 22.5f || degree > 337.5f && degree <= 360.0f)
            _lookDireation = LookDireation.DEGREE_0;
        else if (degree > 22.5f && degree <= 67.5f)
            _lookDireation = LookDireation.DEGREE_45;
        else if (degree > 67.5f && degree <= 112.5f)
            _lookDireation = LookDireation.DEGREE_90;
        else if (degree > 112.5f && degree <= 157.5f)
            _lookDireation = LookDireation.DEGREE_135;
        else if (degree > 157.5f && degree <= 202.5f)
            _lookDireation = LookDireation.DEGREE_180;
        else if (degree > 202.5f && degree <= 247.5f)
            _lookDireation = LookDireation.DEGREE_225;
        else if (degree > 247.5f && degree <= 292.5f)
            _lookDireation = LookDireation.DEGREE_270;
        else if (degree > 292.5f && degree <= 337.5f)
            _lookDireation = LookDireation.DEGREE_315;

    }

}

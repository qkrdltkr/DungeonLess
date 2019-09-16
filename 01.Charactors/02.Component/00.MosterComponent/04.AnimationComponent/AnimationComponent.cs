using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationComponent : IComponent {
    protected Animator _animator;
    public Condition CurrentCondition;
    protected Animator _weaponAnimator;
    protected string[] _conditions;
    protected Vector3 _targetpos;
    protected Transform _origin;
    protected enum LookDireation {
        NONPASS = -1,
        DEGREE_0, DEGREE_45, DEGREE_90, DEGREE_135,
        DEGREE_180, DEGREE_225, DEGREE_270, DEGREE_315
    }
    protected LookDireation _lookDireation;
    public AnimationComponent(Animator anim) {
        this._animator = anim;
    }
    public abstract void Run(float dt);
    public void SetTarget(Transform origin, Transform target) {
        _origin = origin;
        _targetpos = target.position;
    }
    public void SetTarget(Transform origin, Vector3 targetpos) {
        _origin = origin;
        _targetpos = targetpos;
    }
    protected void ConvertConditionWeaponBool(string target) {
        for (int i = 0; i < _conditions.Length; ++i){
            if (_conditions[i].Equals(target)) _weaponAnimator.SetBool(target, true);
            else _weaponAnimator.SetBool(_conditions[i], false);
        }
    }
    protected void ConvertConditionBool(string target) {
        for (int i = 0; i < _conditions.Length; ++i) {
            if (_conditions[i].Equals(target)) _animator.SetBool(target, true);
            else _animator.SetBool(_conditions[i], false);
        }
    }
    protected void ConvertCondition(string target) {
        for (int i = 0; i < _conditions.Length; ++i) {
            if (_conditions[i].Equals(target)) _animator.SetFloat(target, 1.0f);
            else _animator.SetFloat(_conditions[i], 0.0f);
        }
    }
    protected void SetState() {
        switch (CurrentCondition.currentCondition){
            case Condition.kIdle:   ConvertConditionBool("IsIdle");   break;
            case Condition.kRun:    ConvertConditionBool("IsMove");   break;
            case Condition.kDead:   ConvertConditionBool("IsDead");   break;
            case Condition.kAttack: ConvertConditionBool("IsAttack"); break;
        }
    }
    protected void SetWeaponState() {
        switch (CurrentCondition.currentCondition) {
            case Condition.kIdle: ConvertConditionWeaponBool("IsIdle"); break;
            case Condition.kRun: ConvertConditionWeaponBool("IsIdle"); break;
            case Condition.kAttack: ConvertConditionWeaponBool("IsAttack"); break;
        }
    }
}

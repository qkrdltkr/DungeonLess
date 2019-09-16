using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationComponent : AnimationComponent {
    public PlayerAnimationComponent(Animator anim, string[] conditions) : base(anim) {
        _conditions = conditions;
    }

    public Transform Player { get; set; }

    public override void Run(float dt) {
        if (!_animator.gameObject.activeInHierarchy) return;
        SetDir();
        SetState();
        switch (CurrentCondition.currentCondition) {
            case Condition.kIdle:
                switch (_lookDireation)  {
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
            case Condition.kRun:
                switch (_lookDireation) {
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
            case Condition.kDead:
                ConvertConditionBool("IsDead");
                break;

            case Condition.kNonpass:
                ConvertCondition("");
                break;
        }
    }
    private void SetDir() {
        float degree = Utility.GetAngle(Player.position, MainCamController.MousePosition);
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

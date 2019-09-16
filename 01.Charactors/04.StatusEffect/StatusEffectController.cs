using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StatusEffectKind {
    public static int maxStatusEffect = 3;
    public const int kNoStatusEffect = -1;


    public const int kBurning = 0;
    public const int kStun = 1;
    public const int kSlow = 2;

}
public class StatusEffectController {
   
    private List<StatusEffect> _statusEffects = new List<StatusEffect>(); //자기한테 걸린 상태이상들
    StatusEffect _statusEffect;
    StatusIcon _statusIcon;
    GameObject currentBurningObj;

    public void AddStatusEffect(int stautsEffetKind, Transform target) {
        _statusIcon = GameObject.Find("StatusUI").GetComponent<StatusIcon>();
        StatusEffect statusEffect = null;
        switch (stautsEffetKind) {
            case StatusEffectKind.kBurning:
                statusEffect = Burning(target); break;

            case StatusEffectKind.kStun: break;

            case StatusEffectKind.kSlow: break;
        }
        InsertStatusEffect(statusEffect);

    }
    public void RemoveStatusEffect(int kind){

        for (int i = 0; i < _statusEffects.Count; i++){
            if (_statusEffects[i].Kind == kind && _statusEffects[i].isFinished()) _statusEffects.RemoveAt(i);
        }
    }
    private void InsertStatusEffect(StatusEffect statusEffect) {

        if (_statusEffects.Count == 0) {
            _statusEffects.Add(statusEffect);
            _statusIcon.SetStatusEffect(_statusEffects, _statusEffects[0]._durationTime);
        }
        else
        {
            for (int i = 0; i < _statusEffects.Count; i++)
            {
                if (_statusEffects[i].Kind != statusEffect.Kind)
                {
                    _statusEffects.Add(statusEffect);
                }
                else
                    _statusEffects[i]._durationTime = 5f; //변수로 치환

                _statusIcon.SetStatusEffect(_statusEffects, _statusEffects[i]._durationTime);
                //현재 리스트 전달 및 지속시간 전달
            }
        }
    }
    public List<StatusEffect> GetStatusEffect(){
        return _statusEffects;
    }
    public StatusEffect Burning(Transform target)
    {
        GameObject burningObject = null;

        if (_statusEffects.Count == 0) burningObject = StatusPoolManager.Instance.PopStatusEffect(StatusEffectKind.kBurning);
        for (int i = 0; i < _statusEffects.Count; i++) {
            //2번생성되는 것을 방지
            if (_statusEffects[i].Kind == StatusEffectKind.kBurning) burningObject = currentBurningObj;
            else burningObject = StatusPoolManager.Instance.PopStatusEffect(StatusEffectKind.kBurning);
        }
        currentBurningObj = burningObject;
        _statusEffect = burningObject.GetComponent<BurningStatusEffect>();
        _statusEffect.BurningTarget(target);

        return _statusEffect;
    }
}
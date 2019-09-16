using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningStatusEffect : StatusEffect{
    private bool isBurn = false;
    private float _burningTIme = 0f;
    protected override void Initialize(){
        this.Kind = StatusEffectKind.kBurning;
        StatusPoolManager.Instance.SetStatusEffect(this.gameObject, this.transform.parent);
        this.gameObject.SetActive(false);
    }

    public override void BurningTarget(Transform target) {
        _targetPos = target;
        this.gameObject.SetActive(true); 
        _durationTime = 5f;
      
        //5초간 적을 불태움
        isBurn = true;
        Debug.Log("화상상태이상 시작");
    }
    public override bool isFinished() {
        return !isBurn; //불꺼짐
    }
    private void Burning(Transform target, float dt){
        _targetStats = target.gameObject.GetComponent<PlayerController>().GetStats();
        PlayerController playerController;
        playerController = _targetPos.gameObject.GetComponent<PlayerController>();
        this.transform.position = _targetPos.transform.position;
        if (_durationTime <= 0) {
            //불꺼짐
            isBurn = false;
            StatusPoolManager.Instance.RechargeStatusEffect(this.gameObject, this.transform.parent);
            //상태이상 리스트에서 삭제
            playerController.RemoveStatusEffect(StatusEffectKind.kBurning);
            Debug.Log("화상상태이상 끝");
            return;
        }
        _durationTime -= dt;

        _burningTIme += dt;
        if (_burningTIme > 1f) {
            //1초마다 적을 불태움
            _targetStats.HeatPoint -= 5; //나중에 치환
            _burningTIme = 0;
        }
    }
    public override void OnFixedUpdate(float dt) {
        if (isBurn) Burning(_targetPos, dt);
    }
}
